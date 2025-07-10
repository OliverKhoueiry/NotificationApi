using BusinessLayer;
using CommonLayer.Common;
using CommonLayer.Models;
using Microsoft.AspNetCore.Authorization;
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
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var result = await _businessHandler.RegisterAsync(request);
            return result.Code == ResponseMessages.SuccessCode ? Ok(result) : BadRequest(result);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var (response, accessToken, refreshToken) = await _businessHandler.LoginAsync(request);

            if (response.Code != ResponseMessages.SuccessCode)
                return Unauthorized(response);

            return Ok(new
            {
                response.Code,
                response.Description,
                AccessToken = accessToken,
                RefreshToken = refreshToken
            });
        }

        [HttpPost("forgetpassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgetPassword([FromBody] string email)
        {
            var result = await _businessHandler.ForgetPasswordAsync(email);
            return result.Code == ResponseMessages.SuccessCode ? Ok(result) : BadRequest(result);
        }

        [HttpPost("refreshtoken")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
        {
            var (response, newAccessToken, newRefreshToken) = await _businessHandler.RefreshTokenAsync(refreshToken);

            if (response.Code != ResponseMessages.SuccessCode)
                return Unauthorized(response);

            return Ok(new
            {
                response.Code,
                response.Description,
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            });
        }
    }
}
