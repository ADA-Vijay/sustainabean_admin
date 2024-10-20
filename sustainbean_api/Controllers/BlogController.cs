using Microsoft.AspNetCore.Mvc;
using sustainbean_api.Models;
using sustainbean_api.Repository;
using System.Data;

namespace sustainbean_api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly IBlogRepository _blogRepository;

        public BlogController(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        [HttpGet]
        [Route("GetAllBlogsById/{blogId}")]
        public async Task<ActionResult<Tag>> GetBlogById(int blogId)
        {
            var tag = await _blogRepository.GetBlogByIdAsync(blogId);
            if (tag == null)
            {
                return NotFound();
            }
            return Ok(tag);
        }

        [HttpGet]
        [Route("GetBlogsBySlug/{slug}")]
        public async Task<ActionResult<Tag>> GetBlogBySlug(string slug)
        {
            var tag = await _blogRepository.GetBlogBySlugAsync(slug);
            if (tag == null)
            {
                return NotFound();
            }
            return Ok(tag);
        }

        [HttpGet]
        [Route("GetBlogsByCategory/{category}")]
        public async Task<ActionResult<Tag>> GetBlogByCategoryAsync(string category)
        {
            var tag = await _blogRepository.GetBlogByCategoryAsync(category);
            if (tag == null)
            {
                return NotFound();
            }
            return Ok(tag);
        }

        [HttpPost]
        [Route("AddBlog")]
        public async Task<IActionResult> CreateBlog(Blog blog)
        {
            await _blogRepository.AddBlogAsync(blog);
            return Ok(blog);
        }

        [HttpPost]
        [Route("UpdateBlog")]
        public async Task<IActionResult> UpdateBlog(Blog blog)
        {

            await _blogRepository.UpdateBlogAsync(blog);
            return Ok(blog);
        }

        [HttpGet]
        [Route("GetAllBlogs")]
        public async Task<IActionResult> GetBlogs()
        {
            var result = await _blogRepository.GetBlogs();
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

            var updated = await _blogRepository.UpdateBlogStatusAsync(id, status);
            if (!updated)
            {
                return NotFound($"Tag with ID {id} not found.");
            }

            return Ok(); // Status 204 No Content indicates the update was successful
        }
    }
}

