using CatalogWebApiSystem.DataAccess.Interfaces;
using CatalogWebApiSystem.Domain.Models;
using CatalogWebApiSystem.Filters;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CatalogWebApiSystem.Controllers
{
    [ServiceFilter(typeof(ApiLoggingResultFilter))]
    [Route("[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork _uow;


        public ProductsController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _uow.ProductRepository.GetAllAsync();

            return Ok(products);

        }

        [HttpGet("{id:int}", Name = "GetProduct")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            if (id < 1 || id > 9999) // teste de exceção
                throw new Exception("Id is out of range");

            var product = await _uow.ProductRepository.GetAsync(p => p.ProductId == id);

            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            if (product is null)
                return BadRequest();

            var categoryExists = (await _uow.CategoryRepository.GetByIdAsync(product.CategoryId)) is not null;

            if (!categoryExists)
                return BadRequest($"Category with id {product.CategoryId} not found!");

            await _uow.ProductRepository.CreateAsync(product);
            await _uow.CommitAsync();

            return CreatedAtAction(nameof(GetProduct), new { id = product.ProductId }, product);
        }

        [HttpPut("{id:int}")] // constraint -> restrição
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (product is null)
                return BadRequest();

            if (id != product.ProductId)
                return BadRequest("Product id don't match.");

            var categoryExists = (await _uow.CategoryRepository.GetByIdAsync(product.CategoryId)) is not null;

            if (!categoryExists)
                return BadRequest($"Category with id {product.CategoryId} not found!");


            try
            {
                await _uow.ProductRepository.UpdateAsync(product);
                await _uow.CommitAsync();
            }
            catch (Exception)
            {
                return BadRequest($"Fail when updating product with id {id}.");
            }

            //return NoContent();
            return Ok(product);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _uow.ProductRepository.GetAsync(p => p.ProductId == id);

            if (product == null)
                return NotFound();

            try
            {
                await _uow.ProductRepository.DeleteAsync(product);
                await _uow.CommitAsync();
            }
            catch (Exception)
            {
                return BadRequest("Fail when deleting product with id {id}.");
            }

            return NoContent();
        }
    }
}
