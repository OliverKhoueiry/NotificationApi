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
    }

    //public class PermissionRequest
    //{
    //    public int RoleId { get; set; }
    //    public int SectionId { get; set; }
    //    public string Action { get; set; }
    //}
}
