using BusinessLayer;
using CommonLayer.Common;
using CommonLayer.Models;
using Microsoft.AspNetCore.Mvc;
using NotificationApi.Services;

namespace NotificationApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly IBusinessHandler _businessHandler;
        private readonly JwtService _jwtService;

        public NotificationController(IBusinessHandler businessHandler, JwtService jwtService)
        {
            _businessHandler = businessHandler;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var result = await _businessHandler.RegisterAsync(request);

            if (result.Code == ResponseMessages.SuccessCode)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var (result, user) = await _businessHandler.LoginAsync(request);

            if (result.Code != ResponseMessages.SuccessCode || user == null)
                return Ok(result);

            var token = _jwtService.GenerateToken(user);

            return Ok(new
            {
                result.Code,
                result.Description,
                token
            });
        }

        [HttpPost("forgetpassword")]
        public async Task<IActionResult> ForgetPassword([FromBody] string email)
        {
            var result = await _businessHandler.ForgetPasswordAsync(email);

            if (result.Code == ResponseMessages.SuccessCode)
                return Ok(result);

            return BadRequest(result);
        }
    }
}
