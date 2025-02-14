﻿using CatalogWebApiSystem.DataAccess.Interfaces;
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
        private readonly IProductRepository _repository;

        public ProductsController(IProductRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _repository.GetAllAsync();

            return Ok(products);

        }

        [HttpGet("{id:int}", Name = "GetProduct")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            if (id < 1 || id > 9999) // teste de exceção
                throw new Exception("Id is out of range");

            var product = await _repository.GetAsync(p => p.ProductId == id);

            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            if (product is null)
                return BadRequest();

            await _repository.CreateAsync(product);

            return CreatedAtAction(nameof(GetProduct), new { id = product.ProductId }, product);
        }

        [HttpPut("{id:int}")] // constraint -> restrição
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (product is null)
                return BadRequest();

            if (id != product.ProductId)
                return BadRequest("Product id don't match.");

            try
            {
                await _repository.UpdateAsync(product);
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
            var product = await _repository.GetAsync(p => p.ProductId == id);

            if (product == null)
                return NotFound();

            try
            {
                await _repository.DeleteAsync(product);
            }
            catch (Exception)
            {
                return BadRequest("Fail when deleting product with id {id}.");
            }

            return NoContent();
        }
    }
}
