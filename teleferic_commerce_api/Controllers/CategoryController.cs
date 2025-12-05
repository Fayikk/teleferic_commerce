using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using teleferic_commerce_core.ApplicationServices.Interfaces;
using teleferic_commerce_core.DTO.Category;

namespace teleferic_commerce_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost("CreateCategory")]
        public async Task<IActionResult> AddCategory(CreateCategoryDTO dto)
        {
            var response = await _categoryService.CreateCategory(dto);
            if (response.IsSuccess)
            {
                return Ok(response);

            }
            return BadRequest(response);
        }

        [HttpGet("GetAllCategories")]
        public async Task<IActionResult> GetAllCategories()
        {
            var response = await _categoryService.GetCategories();
            if (response.IsSuccess)
            {
                return Ok(response);

            }
            return BadRequest(response);
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetCategoryById(Guid id)
        {
            var response = await _categoryService.GetCategory(id);
            if (response.IsSuccess)
            {
                return Ok(response);

            }
            return BadRequest(response);
        }

        [HttpPut("UpdateCategory/{id}")]
        public async Task<IActionResult> UpdateCategory(Guid id, UpdateCategoryDTO dto)
        {
            var response = await _categoryService.UpdateCategory(id, dto);
            if (response.IsSuccess)
            {
                return Ok(response);

            }
            return BadRequest(response);
        }

        [HttpDelete("DeleteCategory/{id}")]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            var response = await _categoryService.DeleteCategory(id);
            if (response.IsSuccess)
            {
                return Ok(response);

            }
            return BadRequest(response);
        }
    }
}
