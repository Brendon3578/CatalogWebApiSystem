using AutoMapper;
using CatalogWebApiSystem.Application.DTOs;
using CatalogWebApiSystem.DataAccess.Interfaces;
using CatalogWebApiSystem.Domain.Models;
using CatalogWebApiSystem.Filters;
using Microsoft.AspNetCore.JsonPatch;
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
        private readonly IMapper _mapper;


        public ProductsController(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts()
        {
            var products = await _uow.ProductRepository.GetAllAsync();

            var productsDtos = _mapper.Map<IEnumerable<ProductDTO>>(products);

            return Ok(productsDtos);
        }

        [HttpGet("{id:int}", Name = "GetProduct")]
        public async Task<ActionResult<ProductDTO>> GetProduct(int id)
        {
            if (id < 1 || id > 9999) // teste de exceção
                throw new Exception("Id is out of range");

            var product = await _uow.ProductRepository.GetAsync(p => p.ProductId == id);

            if (product == null)
                return NotFound();

            var productDto = _mapper.Map<ProductDTO>(product);

            return Ok(productDto);
        }



        [HttpPost]
        public async Task<ActionResult<ProductDTO>> PostProduct(ProductDTO productDto)
        {
            if (productDto is null)
                return BadRequest();

            var product = _mapper.Map<Product>(productDto);
            product.CreatedOn = DateTime.Now;


            var categoryExists = (await _uow.CategoryRepository.GetByIdAsync(product.CategoryId)) is not null;

            if (!categoryExists)
                return BadRequest($"Category with id {product.CategoryId} not found!");

            await _uow.ProductRepository.CreateAsync(product);
            await _uow.CommitAsync();

            var createdProductDto = _mapper.Map<ProductDTO>(product);

            return CreatedAtAction(nameof(GetProduct), new { id = createdProductDto.ProductId }, createdProductDto);
        }

        [HttpPut("{id:int}")] // constraint -> restrição
        public async Task<ActionResult<ProductDTO>> PutProduct(int id, ProductDTO productDto)
        {
            if (productDto is null)
                return BadRequest();

            var product = _mapper.Map<Product>(productDto);

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
            return Ok(productDto);
        }

        [HttpPatch("{id:int}/UpdatePartial")]
        public async Task<ActionResult<ProductDTOUpdateResponse>> PatchProduct(int id, JsonPatchDocument<ProductDTOUpdateRequest> patchDocument)
        {
            if (patchDocument is null || id <= 0)
                return BadRequest();

            var product = await _uow.ProductRepository.GetAsync(p => p.ProductId == id);

            if (product == null)
                return NotFound();

            var request = _mapper.Map<ProductDTOUpdateRequest>(product);

            patchDocument.ApplyTo(request, ModelState);

            if (!ModelState.IsValid || !TryValidateModel(request))
                return ValidationProblem(ModelState);

            _mapper.Map(request, product);

            try
            {
                await _uow.ProductRepository.UpdateAsync(product);
                await _uow.CommitAsync();
            }
            catch (Exception)
            {
                return BadRequest($"Fail when updating product with id {id}.");
            }

            var response = _mapper.Map<ProductDTOUpdateResponse>(product);

            return Ok(response);
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
