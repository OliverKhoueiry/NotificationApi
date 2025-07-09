using BusinessLayer;
using CommonLayer.Models;
using Microsoft.AspNetCore.Mvc;

namespace NotificationApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly IBusinessHandler _businessHandler;

        public NotificationController(IBusinessHandler businessHandler)
        {
            _businessHandler = businessHandler;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var result = await _businessHandler.RegisterAsync(request);
            if (result == "Registration successful.")
                return Ok(result);

            return BadRequest(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _businessHandler.LoginAsync(request);
            if (user == null)
                return Unauthorized("Invalid credentials.");

            return Ok(new
            {
                user.Id,
                user.Username,
                user.Email
            });
        }
    }
}
