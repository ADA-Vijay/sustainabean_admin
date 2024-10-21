using Microsoft.AspNetCore.Mvc;
using sustainbean_api.Models;
using sustainbean_api.Repository;
using System.Data;

namespace sustainbean_api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        [Route("GetAllCategoryById/{categoryId}")]
        public async Task<ActionResult<Tag>> GetCategoryById(int categoryId)
        {
            var tag = await _categoryRepository.GetCategoryByIdAsync(categoryId);
            if (tag == null)
            {
                return NotFound();
            }
            return Ok(tag);
        }

        [HttpPost]
        [Route("AddCategory")]
        public async Task<IActionResult> CreateCategory(Category category)
        {
            await _categoryRepository.AddCategoryAsync(category);
            return Ok(category);
        }

        [HttpPost]
        [Route("UpdateCategory")]
        public async Task<IActionResult> UpdateCategory(Category category)
        {

            await _categoryRepository.UpdateCategoryAsync(category);
            return Ok(category);
        }

        [HttpPost]
        [Route("GetAllCategory")]
        public async Task<IActionResult> GetCategory()
        {
            var result = await _categoryRepository.GetCategories();
            return Ok(result);
        }

        [HttpGet]
        [Route("GetAllCategoryList")]
        public async Task<IActionResult> GetAllCategoryList()
        {
            var result = await _categoryRepository.GetAllCategoriesAsync();
            return Ok(result);
        }

        // PUT: api/tags/{id}/status
        [HttpPost]
        [Route("UpdateStatus/{id}/{status}")]
        public async Task<IActionResult> UpdateStatus(int id, bool status)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid tag ID.");
            }

            var updated = await _categoryRepository.UpdateCategoryStatusAsync(id, status);
            if (!updated)
            {
                return NotFound($"Tag with ID {id} not found.");
            }

            return Ok(); // Status 204 No Content indicates the update was successful
        }
    }
}

