using Microsoft.AspNetCore.Mvc;
using CommonLayer.Models;
using BusinessLayer;
using CommonLayer.Common;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace NotificationApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IBusinessHandler _businessHandler;
        private readonly JwtService _jwtService;

        public UserController(IBusinessHandler businessHandler, JwtService jwtService)
        {
            _businessHandler = businessHandler;
            _jwtService = jwtService;
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

            var (response, accessToken, refreshToken, permissions) = await _businessHandler.LoginAsync(request);

            if (response.Code != ResponseMessages.SuccessCode)
                return Unauthorized(response);

            return Ok(new
            {
                response.Description,
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                Sections = permissions
                    .GroupBy(p => p.Section)
                    .Select(g => new
                    {
                        Name = g.Key,
                        Actions = g.Select(p => p.Action).Distinct().ToList()
                    }).ToList()
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

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _businessHandler.ResetPasswordAsync(request);
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

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] LogoutRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _businessHandler.LogoutAsync(request.RefreshToken);
            return Ok(response);
        }
        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _businessHandler.GetAllUsersAsync();
            return Ok(users);
        }

        // ✅ New endpoint: Get permissions for a section based on token + section name
        [HttpPost("section")]
        public async Task<IActionResult> GetSectionPermissions([FromBody] SectionRequest request)
        {
            if (string.IsNullOrEmpty(request.SectionName))
                return BadRequest(new ApiResponse(ResponseMessages.ValidationErrorCode, "SectionName is required."));

            var authHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            if (authHeader == null || !authHeader.StartsWith("Bearer "))
                return Unauthorized(new ApiResponse(ResponseMessages.ErrorCode, "Missing or invalid Authorization header."));

            var token = authHeader.Substring("Bearer ".Length).Trim();
            var principal = _jwtService.ValidateToken(token);

            if (principal == null)
                return Unauthorized(new ApiResponse(ResponseMessages.ErrorCode, "Invalid token."));

            var roleClaim = principal.FindFirst("Role");
            if (roleClaim == null)
                return Unauthorized(new ApiResponse(ResponseMessages.ErrorCode, "Role not found in token."));

            var roleName = roleClaim.Value;

            var roleSection = await _businessHandler.GetRoleSectionByNameAsync(roleName, request.SectionName);

            if (roleSection == null)
                return NotFound(new ApiResponse(ResponseMessages.ErrorCode, "Section not found or no permissions."));

            var actions = new[]
            {
                roleSection.IsView ? "View" : null,
                roleSection.IsAdd ? "Add" : null,
                roleSection.IsUpdate ? "Update" : null,
                roleSection.IsDelete ? "Delete" : null
            }.Where(a => a != null).ToList();

            var response = new
            {
                statusCode = new
                {
                    code = ResponseMessages.SuccessCode,
                    message = "Permissions fetched successfully",
                    section = new
                    {
                        id = roleSection.IdSection,
                        name = request.SectionName,
                        actions
                    }
                }
            };

            return Ok(response);
        }
    }

    public class SectionRequest
    {
        public string SectionName { get; set; }
    }
}
