using CatalogWebApiSystem.Domain.Models;
using CatalogWebApiSystem.Filters;
using CatalogWebApiSystem.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CatalogWebApiSystem.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [ServiceFilter(typeof(ApiLoggingResultFilter))]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _repository;

        public CategoriesController(ICategoryRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            var categories = await _repository.GetCategoriesAsync();
            return Ok(categories);
        }

        [HttpGet("{id:int}", Name = "GetCategory")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            if (id < 1 || id > 9999) // teste de exceção
                throw new Exception("Id is out of range");

            var category = await _repository.GetCategoryAsync(id);

            if (category == null)
                return NotFound();

            return Ok(category);
        }


        [HttpGet("{id:int}/products")]
        public async Task<ActionResult<IEnumerable<Product>>> GetCategoryProducts(int id)
        {
            var products = await _repository.GetProductsAsync(id);

            if (!products.Any())
                return NotFound();

            return Ok(products);
        }

        [HttpPost]
        public async Task<ActionResult<Category>> PostCategory(Category category)
        {
            await _repository.CreateAsync(category);

            return CreatedAtAction(nameof(GetCategory), new { id = category.CategoryId }, category);
        }

        [HttpPut("{id:int}")] // constraint -> restrição
        public async Task<IActionResult> PutCategory(int id, Category category)
        {
            if (id != category.CategoryId)
                return BadRequest("Category id don't match.");

            try
            {
                await _repository.UpdateAsync(category);
            }
            catch (DbUpdateConcurrencyException)
            {
                var categoryExists = await _repository.CategoryExistsAsync(id);

                if (!categoryExists)
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

            var category = await _repository.GetCategoryAsync(id);

            if (category == null) return NotFound();

            var deletedCategory = await _repository.DeleteAsync(id);

            return NoContent();
        }

    }
}
