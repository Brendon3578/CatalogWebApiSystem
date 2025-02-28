using AutoMapper;
using CatalogWebApiSystem.Application.DTOs;
using CatalogWebApiSystem.Application.Pagination;
using CatalogWebApiSystem.DataAccess.Interfaces;
using CatalogWebApiSystem.Domain.Models;
using CatalogWebApiSystem.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace CatalogWebApiSystem.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [ServiceFilter(typeof(ApiLoggingResultFilter))]
    public class CategoriesController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public CategoriesController(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetCategories()
        {
            var categories = await _uow.CategoryRepository.GetAllAsync();

            var categoriesDto = _mapper.Map<IEnumerable<CategoryDTO>>(categories);

            return Ok(categoriesDto);
        }

        [HttpGet("{id:int}", Name = "GetCategory")]
        public async Task<ActionResult<CategoryDTO>> GetCategory(int id)
        {
            if (id < 1 || id > 9999) // teste de exceção
                throw new Exception("Id is out of range");

            var category = await _uow.CategoryRepository.GetAsync(c => c.CategoryId == id);

            if (category is null)
                return NotFound();

            var categoryDto = _mapper.Map<CategoryDTO>(category);

            return Ok(categoryDto);
        }


        [HttpGet("{id:int}/Products")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProductsByCategory(int id)
        {
            if (await CategoryExists(id) == false)
                return NotFound();

            var products = await _uow.ProductRepository.GetProductsByCategoryAsync(id);

            var productsDtos = _mapper.Map<IEnumerable<ProductDTO>>(products);

            return Ok(productsDtos);
        }


        [HttpGet("{id:int}/Products/pagination")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProductsByCategoryAsync(int id, [FromQuery] ProductParameters productParams)
        {
            if (await CategoryExists(id) == false)
                return NotFound();

            var products = await _uow.ProductRepository.GetProductsByCategoryAsync(id, productParams);

            var metadata = PaginationResponseHeader.ToPaginationResponseHeader(products);

            var jsonMetadata = JsonSerializer.Serialize(metadata, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            Response.Headers.Append("X-Pagination", jsonMetadata);

            var productsDtos = _mapper.Map<IEnumerable<ProductDTO>>(products);

            return Ok(productsDtos);
        }

        [HttpPost]
        public async Task<ActionResult<CategoryDTO>> PostCategory(CategoryDTO categoryDto)
        {

            if (categoryDto is null)
                return BadRequest("Category is null.");

            var category = _mapper.Map<Category>(categoryDto);

            await _uow.CategoryRepository.CreateAsync(category);

            await _uow.CommitAsync();

            var createdCategoryDto = _mapper.Map<CategoryDTO>(category);

            return CreatedAtAction(nameof(GetCategory), new { id = createdCategoryDto.CategoryId }, createdCategoryDto);
        }

        [HttpPut("{id:int}")] // constraint -> restrição
        public async Task<IActionResult> PutCategory(int id, CategoryDTO categoryDto)
        {
            if (id != categoryDto.CategoryId)
                return BadRequest("Category id don't match.");

            var category = _mapper.Map<Category>(categoryDto);

            if (category is null)
                return BadRequest("Category is null.");

            try
            {
                await _uow.CategoryRepository.UpdateAsync(category);
                await _uow.CommitAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                var updatedCategory = await _uow.CategoryRepository.GetByIdAsync(id);

                if (updatedCategory is null)
                    return NotFound();
                else
                    throw;
            }

            //return NoContent();
            return Ok(category);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _uow.CategoryRepository.GetAsync(c => c.CategoryId == id);

            if (category is null)
                return NotFound();

            var deletedCategory = await _uow.CategoryRepository.DeleteAsync(category);
            await _uow.CommitAsync();

            return NoContent();
        }

        private async Task<bool> CategoryExists(int id) =>
            await _uow.CategoryRepository.GetByIdAsync(id) is not null;

    }
}
