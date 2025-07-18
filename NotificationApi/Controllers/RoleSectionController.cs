using BusinessLayer;
using CommonLayer.Common;
using CommonLayer.Models;
using Microsoft.AspNetCore.Mvc;

namespace NotificationApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class RoleSectionController : ControllerBase
    {
        private readonly IBusinessHandler _businessHandler;

        public RoleSectionController(IBusinessHandler businessHandler)
        {
            _businessHandler = businessHandler;
        }

        [HttpPost("AddRoleSection")]
        public async Task<IActionResult> AddRoleSection([FromBody] RoleSection request)
        {
            var response = await _businessHandler.AddRoleSectionAsync(request);
            return StatusCode(response.Code == ResponseMessages.SuccessCode ? 200 : 400, response);
        }

        [HttpPut("UpdateRoleSection/{id}")]
        public async Task<IActionResult> UpdateRoleSection(int id, [FromBody] RoleSection request)
        {
            request.Id = id;
            var response = await _businessHandler.UpdateRoleSectionAsync(request);
            return StatusCode(response.Code == ResponseMessages.SuccessCode ? 200 : 400, response);
        }

        [HttpDelete("DeleteRoleSection/{id}")]
        public async Task<IActionResult> DeleteRoleSection(int id)
        {
            var response = await _businessHandler.DeleteRoleSectionAsync(id);
            return StatusCode(response.Code == ResponseMessages.SuccessCode ? 200 : 400, response);
        }

        [HttpGet("GetRoleSections/{roleId}")]
        public async Task<IActionResult> GetRoleSections(int roleId)
        {
            var result = await _businessHandler.GetRoleSectionsAsync(roleId);
            return Ok(result);
        }
    }
}
