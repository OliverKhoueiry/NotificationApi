using BusinessLayer;
using CommonLayer.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;



[ApiController]
[Route("api/[controller]")]
// [Authorize(Roles = "SuperAdmin")] // Uncomment if only SuperAdmin should promote
public class PromoteController : ControllerBase
{
    private readonly IBusinessHandler _businessHandler;

    public PromoteController(IBusinessHandler businessHandler)
    {
        _businessHandler = businessHandler;
    }

    [HttpPut("promote/{userId}")]
    public async Task<IActionResult> PromoteUserToRole(int userId, [FromQuery] string roleName)
    {
        if (string.IsNullOrWhiteSpace(roleName))
        {
            return BadRequest(new ApiResponse(ResponseMessages.ValidationErrorCode, "RoleName is required."));
        }
        var result = await _businessHandler.PromoteUserToRoleAsync(userId, roleName);
        return StatusCode(result.Code == ResponseMessages.SuccessCode ? 200 : 400, result);
    }
}
