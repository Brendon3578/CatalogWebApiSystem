using CatalogWebApiSystem.Application.DTOs.Security;
using CatalogWebApiSystem.Domain.Models;
using CatalogWebApiSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CatalogWebApiSystem.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _config;
        private readonly ILogger<AuthController> _logger;


        public AuthController(ITokenService tokenService, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration config, ILogger<AuthController> logger)
        {
            _tokenService = tokenService;
            _userManager = userManager;
            _roleManager = roleManager;
            _config = config;
            _logger = logger;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            var user = await _userManager.FindByNameAsync(dto.Username!);

            if (user is null)
                return NotFound();

            if (await _userManager.CheckPasswordAsync(user, dto.Password!) == false)
                return Unauthorized();

            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
                {
                    new("id", user.UserName!),
                    new(ClaimTypes.Name, user.UserName!),
                    new(ClaimTypes.Email, user.Email!),
                    new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

            foreach (var userRole in userRoles)
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));

            var token = _tokenService.GenerateAccessToken(authClaims, _config);

            var refreshToken = _tokenService.GenerateRefreshToken();

            _ = int.TryParse(_config["JWT:RefreshTokenValidityInMinutes"], out int refreshTokenValidityInMinutes);

            user.RefreshToken = refreshToken;

            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(refreshTokenValidityInMinutes);

            await _userManager.UpdateAsync(user);

            return Ok(new
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshToken,
                Expiration = token.ValidTo
            });
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO dto)
        {
            var userExists = await _userManager.FindByNameAsync(dto.Username!) != null;

            if (userExists)
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new ResponseDTO { Status = "Error", Message = "User already exists!" }
                );

            var user = new ApplicationUser()
            {
                Email = dto.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = dto.Username
            };

            var result = await _userManager.CreateAsync(user, dto.Password!);

            if (!result.Succeeded)
            {
                var errorMessages = result.Errors.Select(result => result.Description);

                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new ResponseDTO { Status = "Error", Message = $"User creation failed! Please check user details and try again: \n{string.Join("\n", errorMessages)}" }
                );
            }

            return Ok(new ResponseDTO { Status = "Success", Message = "User created successfully!" });
        }

        [HttpPost]
        [Route("RefreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenDTO token)
        {
            if (token?.AccessToken is null || token.RefreshToken is null)
                return BadRequest("Invalid client request");

            var principal = _tokenService.GetPrincipalFromExpiredToken(token.AccessToken, _config);

            if (principal?.Identity?.Name is null)
                return Unauthorized("Invalid access token or refresh token");

            var user = await _userManager.FindByNameAsync(principal.Identity.Name);

            if (user is null || !IsValidRefreshToken(user, token.RefreshToken))
                return Forbid("Invalid access token or refresh token");

            var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims.ToList(), _config);

            var newRefreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            await _userManager.UpdateAsync(user);

            return new ObjectResult(new
            {
                accessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                refreshToken = newRefreshToken,
            });
        }

        [Authorize]
        [HttpPost]
        [Route("Revoke/{username}")]
        public async Task<IActionResult> Revoke(string username)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user == null) return BadRequest("Invalid username");

            user.RefreshToken = null;

            await _userManager.UpdateAsync(user);

            return NoContent();
        }

        [HttpPost]
        [Route("CreateRole")]
        [Authorize(Policy = "SuperAdminOnly")]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            var roleExists = await _roleManager.RoleExistsAsync(roleName);

            if (roleExists)
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new ResponseDTO { Status = "Error", Message = "Role already exists!" }
                );

            var role = new IdentityRole(roleName);
            var result = await _roleManager.CreateAsync(role);

            if (!result.Succeeded)
            {
                _logger.LogInformation(2, "Error when creating new role!");
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseDTO { Status = "Error", Message = $"Issue adding the new {roleName} role!" });
            }

            _logger.LogInformation(1, "Role {roleName} created successfully!", roleName);
            return StatusCode(StatusCodes.Status201Created, new ResponseDTO { Status = "Success", Message = $"Role {roleName} created successfully!" });
        }

        [HttpPost]
        [Route("AddUserToRole")]
        public async Task<IActionResult> AddUserToRole(string email, string roleName)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user is null)
                return StatusCode(
                    StatusCodes.Status404NotFound,
                    new ResponseDTO { Status = "Error", Message = "User not found!" }
                );

            var roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
                return StatusCode(
                    StatusCodes.Status404NotFound,
                    new ResponseDTO { Status = "Error", Message = "Role not found!" }
                );

            var result = await _userManager.AddToRoleAsync(user, roleName);

            if (!result.Succeeded)
            {
                var errorMessages = result.Errors.Select(result => result.Description);

                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new ResponseDTO { Status = "Error", Message = $"Failed to add user to role! {string.Join("\n", errorMessages)}" }
                );
            }

            return Ok(new ResponseDTO { Status = "Success", Message = $"User {email} added to role {roleName} successfully!" });
        }

        private static bool IsValidRefreshToken(ApplicationUser user, string refreshToken)
        {
            var isSameToken = user.RefreshToken == refreshToken;
            var isTokenNotExpired = user.RefreshTokenExpiryTime > DateTime.UtcNow;

            return isSameToken && isTokenNotExpired;
        }
    }
}
