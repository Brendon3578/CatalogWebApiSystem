﻿using CatalogWebApiSystem.Context;
using CatalogWebApiSystem.Domain.Constants;
using CatalogWebApiSystem.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CatalogWebApiSystem.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly CatalogWebApiSystemContext _context;

        public CategoriesController(CatalogWebApiSystemContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            try
            {
                return await _context.Categories.AsNoTracking().ToListAsync();
            }
            catch (Exception)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    ApiErrorMessages.UnableToRecognizeRequest
                );
                throw;
            }
        }

        [HttpGet("{id:int}", Name = "GetCategory")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            if (id < 1 || id > 9999) // teste de exceção
                throw new Exception("Id is out of range");

            var category = await _context.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.CategoryId == id);

            if (category == null)
                return NotFound();

            return category;
        }


        [HttpGet("{id:int}/products")]
        public async Task<ActionResult<IEnumerable<Product>>> GetCategoryProducts(int id)
        {
            var products = await _context.Products
                .AsNoTracking()
                .Where(p => p.CategoryId == id)
                .ToListAsync();

            if (products.Count == 0)
                return NotFound();

            return products;
        }

        [HttpPost]
        public async Task<ActionResult<Category>> PostCategory(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCategory), new { id = category.CategoryId }, category);
        }

        [HttpPut("{id:int}")] // constraint -> restrição
        public async Task<IActionResult> PutCategory(int id, Category category)
        {
            if (id != category.CategoryId)
                return BadRequest("Category id don't match.");

            _context.Entry(category).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
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
            var category = await _context.Categories.FindAsync(id);

            if (category == null) return NotFound();
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CategoryExists(int id) =>
            _context.Categories.AsNoTracking().Any(e => e.CategoryId == id);
    }
}
