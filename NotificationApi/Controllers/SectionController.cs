using BusinessLayer;
using CommonLayer.Common;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace NotificationApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SectionsController : ControllerBase
    {
        private readonly IBusinessHandler _businessHandler;

        public SectionsController(IBusinessHandler businessHandler)
        {
            _businessHandler = businessHandler;
        }

        // 🔥 Get All Sections
        //[HttpGet("all")]
        //public async Task<IActionResult> GetAllSections()
        //{
        //    var sections = await _businessHandler.GetAllSectionsAsync();
        //    return Ok(sections);
        //}

        // 🔥 Add Section
        [HttpPost("AddSection")]
        public async Task<IActionResult> AddSection([FromBody] string name)
        {
            var response = await _businessHandler.AddSectionAsync(name);
            return StatusCode(response.Code == ResponseMessages.SuccessCode ? 200 : 400, response);
        }

        // 🔥 Update Section
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateSection(int id, [FromBody] string name)
        {
            var response = await _businessHandler.UpdateSectionAsync(id, name);
            return StatusCode(response.Code == ResponseMessages.SuccessCode ? 200 : 400, response);
        }

        // 🔥 Delete Section
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteSection(int id)
        {
            var response = await _businessHandler.DeleteSectionAsync(id);
            return StatusCode(response.Code == ResponseMessages.SuccessCode ? 200 : 400, response);
        }
    }
}
