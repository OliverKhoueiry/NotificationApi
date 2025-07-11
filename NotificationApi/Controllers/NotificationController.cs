using Microsoft.AspNetCore.Mvc;
using CommonLayer.Models;
using BusinessLayer;
using System.Threading.Tasks;
using CommonLayer.Common;

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
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _businessHandler.RegisterAsync(request);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var (response, accessToken, refreshToken) = await _businessHandler.LoginAsync(request);
            if (response.Code != ResponseMessages.SuccessCode)
                return Unauthorized(response);

            return Ok(new
            {
                response.Description,
                AccessToken = accessToken,
                RefreshToken = refreshToken
            });
        }

        [HttpPost("forget-password")]
        public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _businessHandler.ForgetPasswordAsync(request.Email);
            return Ok(response);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            var (response, newAccessToken, newRefreshToken) = await _businessHandler.RefreshTokenAsync(request.RefreshToken);
            if (response.Code != ResponseMessages.SuccessCode)
                return Unauthorized(response);

            return Ok(new
            {
                response.Description,
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            });
        }
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _businessHandler.ResetPasswordAsync(request);
            return Ok(response);
        }

    }
}
