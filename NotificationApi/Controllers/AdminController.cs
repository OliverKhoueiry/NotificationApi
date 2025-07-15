using BusinessLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")] // 🔥 Only Admins
public class AdminController : ControllerBase
{
    private readonly IBusinessHandler _businessHandler;

    public AdminController(IBusinessHandler businessHandler)
    {
        _businessHandler = businessHandler;
    }

    [HttpPut("promote/{userId}")]
    public async Task<IActionResult> PromoteUserToAdmin(int userId)
    {
        var result = await _businessHandler.PromoteUserToAdminAsync(userId);
        return Ok(result);
    }
}
