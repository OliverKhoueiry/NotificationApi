using BusinessLayer;
using CommonLayer.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CommonLayer.Models;

namespace NotificationApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    
    public class WebRoleController : ControllerBase
    {
        private readonly IBusinessHandler _businessHandler;

        public WebRoleController(IBusinessHandler businessHandler)
        {
            _businessHandler = businessHandler;
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetWebRoles()
        {
            var roles = await _businessHandler.GetWebRolesAsync();
            return Ok(roles);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddWebRole([FromBody] string name)
        {
            var response = await _businessHandler.AddWebRoleAsync(name);
            return Ok(response);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateWebRole(int id, [FromBody] string name)
        {
            var response = await _businessHandler.UpdateWebRoleAsync(id, name);
            return Ok(response);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteWebRole(int id)
        {
            var response = await _businessHandler.DeleteWebRoleAsync(id);
            return Ok(response);
        }

        [HttpPost("assign-permission")]
        public async Task<IActionResult> AssignPermission([FromBody] PermissionRequest request)
        {
            var response = await _businessHandler.AssignPermissionAsync(request.RoleId, request.SectionId, request.Action);
            return Ok(response);
        }

        [HttpGet("permissions/{roleId}")]
        public async Task<IActionResult> GetRolePermissions(int roleId)
        {
            var permissions = await _businessHandler.GetRolePermissionsAsync(roleId);
            return Ok(permissions);
        }

        [HttpPost("AddRole")]
        public async Task<IActionResult> AddRole([FromBody] AddRoleDto roleDto)
        {
            var response = await _businessHandler.AddRoleAsync(roleDto);
            return StatusCode(response.Code == ResponseMessages.SuccessCode ? 200 : 400, response);
        }
        [HttpPut("UpdateRole/{roleId}")]
        public async Task<IActionResult> UpdateRole(int roleId, [FromBody] AddRoleDto roleDto)
        {
            var response = await _businessHandler.UpdateRoleAsync(roleId, roleDto);
            return StatusCode(response.Code == ResponseMessages.SuccessCode ? 200 : 400, response);
        }
        [HttpGet("roles")]
        public async Task<IActionResult> GetAllRoles()
        {
            var (response, roles) = await _businessHandler.GetAllRolesAsync();

            if (response.Code != ResponseMessages.SuccessCode)
                return BadRequest(response);

            return Ok(new
            {
                Response = response,
                Data = roles
            });
        }
        [HttpGet("roles/{id}")]
        public async Task<IActionResult> GetRoleById(int id)
        {
            var (response, role) = await _businessHandler.GetRoleByIdAsync(id);

            return Ok(new
            {
                response,
                data = role
            });
        }

    }

}
