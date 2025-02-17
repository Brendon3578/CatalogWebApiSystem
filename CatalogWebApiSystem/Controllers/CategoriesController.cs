using CatalogWebApiSystem.Application.DTOs;
using CatalogWebApiSystem.DataAccess.Interfaces;
using CatalogWebApiSystem.Domain.Models;
using CatalogWebApiSystem.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CatalogWebApiSystem.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [ServiceFilter(typeof(ApiLoggingResultFilter))]
    public class CategoriesController : ControllerBase
    {
        private readonly IUnitOfWork _uow;

        public CategoriesController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetCategories()
        {
            var categories = await _uow.CategoryRepository.GetAllAsync();

            var categoriesDto = categories.Select(c => new CategoryDTO()
            {
                CategoryId = c.CategoryId,
                Name = c.Name,
                ImageUrl = c.ImageUrl
            });

            return Ok(categoriesDto);
        }

        [HttpGet("{id:int}", Name = "GetCategory")]
        public async Task<ActionResult<CategoryDTO>> GetCategory(int id)
        {
            if (id < 1 || id > 9999) // teste de exceção
                throw new Exception("Id is out of range");

            var category = await _uow.CategoryRepository.GetAsync(c => c.CategoryId == id);

            if (category == null)
                return NotFound();

            var categoryDto = new CategoryDTO()
            {
                CategoryId = category.CategoryId,
                Name = category.Name,
                ImageUrl = category.ImageUrl
            };

            return Ok(categoryDto);
        }


        [HttpGet("{id:int}/Products")]
        public async Task<IActionResult> GetProductsByCategory(int id)
        {
            var categoryExists = await _uow.CategoryRepository.GetByIdAsync(id);

            if (categoryExists is null)
                return NotFound();

            var products = await _uow.ProductRepository.GetProductsByCategoryAsync(id);

            if (products == null)
                return NotFound();

            return Ok(products);
        }

        [HttpPost]
        public async Task<ActionResult<CategoryDTO>> PostCategory(CategoryDTO categoryDto)
        {
            var category = new Category()
            {
                CategoryId = categoryDto.CategoryId,
                Name = categoryDto.Name,
                ImageUrl = categoryDto.ImageUrl
            };

            await _uow.CategoryRepository.CreateAsync(category);

            await _uow.CommitAsync();

            return CreatedAtAction(nameof(GetCategory), new { id = category.CategoryId }, category);
        }

        [HttpPut("{id:int}")] // constraint -> restrição
        public async Task<IActionResult> PutCategory(int id, CategoryDTO categoryDto)
        {
            if (id != categoryDto.CategoryId)
                return BadRequest("Category id don't match.");

            var category = new Category()
            {
                CategoryId = categoryDto.CategoryId,
                Name = categoryDto.Name,
                ImageUrl = categoryDto.ImageUrl
            };

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

    }
}
