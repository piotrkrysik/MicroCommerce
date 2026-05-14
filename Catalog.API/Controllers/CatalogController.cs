using Catalog.Application.DTOs;
using Catalog.Domain.Entities;
using Catalog.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Catalog.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        // Korzystamy TYLKO z repozytorium, nie z Contextu bezpośrednio
        private readonly IProductRepository _repository;
        private readonly ILogger<CatalogController> _logger;

        public CatalogController(IProductRepository repository, ILogger<CatalogController> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ProductDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
        {
            var products = await _repository.GetProducts();

            // Mapowanie na DTO
            var productsDto = products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                ImageFile = p.ImageFile,
                Summary = p.Summary,
                Price = p.Price,
                CategoryId = p.CategoryId
            });

            return Ok(productsDto);
        }

        [HttpGet("{id:guid}", Name = "GetProduct")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ProductDto), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ProductDto>> GetProductById(Guid id)
        {
            var p = await _repository.GetProduct(id);

            if (p == null)
            {
                _logger.LogError($"Produkt o id: {id}, nie został znaleziony.");
                return NotFound();
            }

            var productDto = new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                ImageFile = p.ImageFile,
                Summary = p.Summary,
                Price = p.Price,
                CategoryId = p.CategoryId
            };

            return Ok(productDto);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ProductDto), (int)HttpStatusCode.Created)] // Zmienione na Created
        public async Task<ActionResult<ProductDto>> CreateProduct([FromBody] CreateProductDto productDto)
        {
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = productDto.Name,
                Summary = productDto.Summary,
                Description = productDto.Description,
                Price = productDto.Price,
                CategoryId = productDto.CategoryId
            };

            await _repository.CreateProduct(product);

            // Mapujemy z powrotem na DTO przed wysłaniem do klienta
            var resultDto = new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                ImageFile = product.ImageFile,
                Summary = product.Summary,
                Price = product.Price,
                CategoryId = product.CategoryId
            };

            return CreatedAtRoute("GetProduct", new { id = product.Id }, resultDto);
        }

        [HttpPut("{id:guid}")] // Warto przekazywać ID w URL dla PUT
        public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] UpdateProductDto productDto)
        {
            var existingProduct = await _repository.GetProduct(id);
            if (existingProduct == null) return NotFound();

            existingProduct.Name = productDto.Name;
            existingProduct.Price = productDto.Price;
            existingProduct.ImageFile = productDto.ImageFile;
            existingProduct.Summary = productDto.Summary;
            existingProduct.Price = productDto.Price;
            existingProduct.CategoryId = productDto.CategoryId;

            return Ok(await _repository.UpdateProduct(existingProduct));
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteProductById(Guid id)
        {
            var result = await _repository.DeleteProduct(id);
            if (!result) return NotFound();

            return Ok(result);
        }
    }
}