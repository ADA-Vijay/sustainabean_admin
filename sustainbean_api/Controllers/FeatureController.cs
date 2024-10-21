using Microsoft.AspNetCore.Mvc;
using sustainbean_api.Models;
using System.Threading.Tasks;

namespace sustainbean_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeatureController : ControllerBase
    {
        private readonly IFeatureRepository _featureRepository;

        public FeatureController(IFeatureRepository featureRepository)
        {
            _featureRepository = featureRepository;
        }

        // GET: api/Feature
        [HttpGet]
        [Route("GetAllFeaturesList")]
        public async Task<IActionResult> GetAllFeatures()
        {
            var features = await _featureRepository.GetAllFeaturesAsync();
            return Ok(features);
        }

        // GET: api/Feature/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFeatureById(int id)
        {
            var feature = await _featureRepository.GetFeatureByIdAsync(id);
            if (feature == null)
            {
                return NotFound();
            }
            return Ok(feature);
        }

        // POST: api/Feature
        [HttpPost("AddFeature")]
        public async Task<IActionResult> AddFeature([FromBody] Feature feature)
        {
            if (feature == null)
            {
                return BadRequest("Feature is null.");
            }

            var createdFeature = await _featureRepository.AddFeatureAsync(feature);
            return CreatedAtAction(nameof(GetFeatureById), new { id = createdFeature.feature_id }, createdFeature);
        }

        // PUT: api/Feature/{id}
        [HttpPost]
        [Route("UpdateFeature")]
        public async Task<IActionResult> UpdateFeature(Feature feature)
        {
            var updatedFeature = await _featureRepository.UpdateFeatureAsync(feature);
            return Ok(updatedFeature);
        }

        [HttpPost]
        [Route("UpdateStatus/{id}/{status}")]
        public async Task<IActionResult> UpdateFeatureStatus(int id, bool status)
        {
            var feature = await _featureRepository.GetFeatureByIdAsync(id);
            if (feature == null)
            {
                return NotFound();
            }

            var result = await _featureRepository.UpdateFeatureStatusAsync(id, status);
            if (!result)
            {
                return StatusCode(500, "Error updating feature status.");
            }

            return Ok("Feature status updated successfully.");
        }

        // POST: api/Feature/GetFeatures
        [HttpPost("GetAllFeatures")]
        public async Task<IActionResult> GetFeatures()
        {
            var features = await _featureRepository.GetFeatures();
            return Ok(features);
        }
    }
}
