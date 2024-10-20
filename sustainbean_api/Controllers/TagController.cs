using Microsoft.AspNetCore.Mvc;
using sustainbean_api.Models;
using sustainbean_api.Repository;
using System.Data;

namespace sustainbean_api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly ITagRepository _tagRepository;

        public TagController(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        [HttpGet]
        [Route("GetAllTagsById/{tagId}")]
        public async Task<ActionResult<Tag>> GetTagById(int tagId)
        {
            var tag = await _tagRepository.GetTagByIdAsync(tagId);
            if (tag == null)
            {
                return NotFound();
            }
            return Ok(tag);
        }

        [HttpPost]
        [Route("AddTag")]
        public async Task<IActionResult> CreateTag(Tag tag)
        {
            await _tagRepository.AddTagAsync(tag);
            return Ok(tag);
        }

        [HttpPost]
        [Route("UpdateTag")]
        public async Task<IActionResult> UpdateTag(Tag tag)
        {

            await _tagRepository.UpdateTagAsync(tag);
            return Ok(tag);
        }

        [HttpGet]
        [Route("GetAllTags")]
        public async Task<IActionResult> GetTags()
        {
            var result = await _tagRepository.GetTags();
            return Ok(result);
        }

        [HttpGet]
        [Route("GetAllTagsList")]
        public async Task<IActionResult> GetAllTagsList()
        {
            var result = await _tagRepository.GetAllTagsAsync();
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

            var updated = await _tagRepository.UpdateTagStatusAsync(id, status);
            if (!updated)
            {
                return NotFound($"Tag with ID {id} not found.");
            }

            return Ok(); // Status 204 No Content indicates the update was successful
        }
    }
}

