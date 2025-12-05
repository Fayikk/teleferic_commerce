using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using teleferic_commerce_core.ApplicationServices.Interfaces;
using teleferic_commerce_core.DTO.Product;

namespace teleferic_commerce_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService productService;

        public ProductController(IProductService productService)
        {
            this.productService = productService;
        }

        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetProducts(Guid categoryId)
        {
            var result = await productService.GetProducts(categoryId);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(Guid id)
        {
            var result = await productService.GetProductById(id);

            if (!result.IsSuccess)
                return NotFound(result);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDTO createProductDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await productService.CreateProduct(createProductDTO);

            if (!result.IsSuccess)
                return BadRequest(result);

            return CreatedAtAction(nameof(GetProductById), new { id = result.Data.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] UpdateProductDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await productService.UpdateProduct(id, dto);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            var result = await productService.DeleteProduct(id);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("{id}/images")]
        public async Task<IActionResult> AddProductImage(Guid id, [FromForm] AddProductImageDto addImageDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await productService.AddProductImage(id, addImageDTO);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete("{productId}/images/{imageId}")]
        public async Task<IActionResult> DeleteProductImage(Guid productId, Guid imageId)
        {
            var result = await productService.DeleteProductImage(productId, imageId);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }
    }
}