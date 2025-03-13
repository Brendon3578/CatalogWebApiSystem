using CatalogWebApiSystem.Application.DTOs.Mappings;
using CatalogWebApiSystem.DataAccess.Context;
using CatalogWebApiSystem.DataAccess.Interfaces;
using CatalogWebApiSystem.DataAccess.Repositories;
using CatalogWebApiSystem.Domain.Models;
using CatalogWebApiSystem.Extensions;
using CatalogWebApiSystem.Filters;
using CatalogWebApiSystem.Logging;
using CatalogWebApiSystem.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SwaggerThemes;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Configuração de Serviços (Startup -> ConfigureServices)

// Controller configuration
builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(ApiExceptionFilter));
})
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
})
.AddNewtonsoftJson();

// Swagger configuration
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Catalog API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = @"JWT Authorization header using the Bearer scheme.
                      Enter 'Bearer' [space] and then your token in the text input below;
                      Example: 'Bearer 12345abcdef'",
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });

});


// Identity configuration
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<CatalogWebApiSystemContext>()
    .AddDefaultTokenProviders();

// Authentication and Authorization configuration

var secretKey = builder.Configuration["JWT:SecretKey"]
    ?? throw new ArgumentException("Invalid secret key!!");

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false; // in prod -> true
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero,
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };
});


// Database configuration
var mySqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<CatalogWebApiSystemContext>(options =>
    options.UseMySql(mySqlConnection,
    ServerVersion.AutoDetect(mySqlConnection)));

// Logging configuration
builder.Logging.AddProvider(new CustomLoggerProvider(
    new CustomLoggerProviderConfiguration { LogLevel = LogLevel.Information }
));

builder.Services.AddScoped<ApiLoggingResultFilter>();

// Repository configuration
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// Unit of Work configuration
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Internal Services configuration
builder.Services.AddScoped<ITokenService, TokenService>();

// AutoMapper configuration
builder.Services.AddAutoMapper(typeof(ModelDTOMappingProfile));


var app = builder.Build();

// Configuração de Middlewares (Startup ->Configure)

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        var apiTitle = builder.Configuration["SwaggerConfiguration:Title"] ?? "Catalog API";
        var theme = @"/swagger/SwaggerDark.css";

        options.DocumentTitle = apiTitle;
        options.InjectStylesheet(theme);
    });

    app.ConfigureExceptionHandler();
}

// Static Files for Swagger configuration
app.UseStaticFiles();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
