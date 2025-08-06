using Microsoft.AspNetCore.Mvc;
using OfflineDemo.Core.Services;
using OfflineDemo.Models.Requests;

namespace OfflineDemoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShortsController : ControllerBase
    {
        private readonly IShortsService _service;
        public ShortsController(IShortsService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllShorts()
        {
            try
            {
                var response = await _service.GetAllShorts();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data: {ex.Message}");
            }
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadShort([FromForm]UploadRequest request)
        {
            try
            {
                await _service.UploadShort(request);
            }
            catch
            {
                return BadRequest("An error occurred while uploading the short. Please ensure both MP4 and image files are provided, and they meet the size requirements.");
            }

            return Ok(new { Message = "Files uploaded to Azure Blob Storage successfully." });
        }
    }
}
