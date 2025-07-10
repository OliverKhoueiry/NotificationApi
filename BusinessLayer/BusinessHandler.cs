using System;
using System.Threading.Tasks;
using CommonLayer.Common;
using CommonLayer.Models;
using DataLayer;

namespace BusinessLayer
{
    public class BusinessHandler : IBusinessHandler
    {
        private readonly IDataHandler _dataHandler;
        private readonly JwtService _jwtService;
        private readonly EmailService _emailService;

        public BusinessHandler(IDataHandler dataHandler, JwtService jwtService, EmailService emailService)
        {
            _dataHandler = dataHandler;
            _jwtService = jwtService;
            _emailService = emailService;
        }

        public async Task<ApiResponse> RegisterAsync(RegisterRequest request)
        {
            var existingUser = await _dataHandler.GetUserByEmailAsync(request.Email);
            if (existingUser != null)
            {
                return ResponseMessages.EmailAlreadyRegistered;
            }

            string hashedPassword = PasswordHasher.HashPassword(request.Password);

            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = hashedPassword,
                CreatedAt = DateTime.UtcNow
            };

            var rowsAffected = await _dataHandler.AddUserAsync(user);
            if (rowsAffected > 0)
            {
                return ResponseMessages.RegistrationSuccessful;
            }

            return new ApiResponse(ResponseMessages.ErrorCode, "Registration failed.");
        }

        public async Task<(ApiResponse, string?, string?)> LoginAsync(LoginRequest request)
        {
            var user = await _dataHandler.GetUserByEmailAsync(request.Email);
            if (user == null || !PasswordHasher.VerifyPassword(request.Password, user.PasswordHash))
            {
                return (ResponseMessages.InvalidCredentials, null, null);
            }

            string accessToken = _jwtService.GenerateAccessToken(user);
            string refreshToken = _jwtService.GenerateRefreshToken();
            DateTime refreshTokenExpiry = DateTime.UtcNow.AddDays(7);

            await _dataHandler.UpdateRefreshTokenAsync(user.Id, refreshToken, refreshTokenExpiry);

            return (ResponseMessages.LoginSuccessful, accessToken, refreshToken);
        }

        public async Task<ApiResponse> ForgetPasswordAsync(string email)
        {
            var user = await _dataHandler.GetUserByEmailAsync(email);
            if (user == null)
            {
                return new ApiResponse(ResponseMessages.ErrorCode, "Email not found.");
            }

            string tempPassword = Guid.NewGuid().ToString().Substring(0, 8);
            string hashedPassword = PasswordHasher.HashPassword(tempPassword);

            user.PasswordHash = hashedPassword;
            await _dataHandler.UpdateRefreshTokenAsync(user.Id, null, DateTime.UtcNow); // Invalidate old refresh token

            await _emailService.SendEmailAsync(email, "Password Reset", $"Your new password is: {tempPassword}");

            return ResponseMessages.ForgetPasswordEmailSent;
        }

        public async Task<(ApiResponse, string?, string?)> RefreshTokenAsync(string refreshToken)
        {
            var user = await _dataHandler.GetUserByRefreshTokenAsync(refreshToken);
            if (user == null || user.RefreshTokenExpiry < DateTime.UtcNow)
            {
                return (ResponseMessages.InvalidRefreshToken, null, null);
            }

            string newAccessToken = _jwtService.GenerateAccessToken(user);
            string newRefreshToken = _jwtService.GenerateRefreshToken();
            DateTime newRefreshTokenExpiry = DateTime.UtcNow.AddDays(7);

            await _dataHandler.UpdateRefreshTokenAsync(user.Id, newRefreshToken, newRefreshTokenExpiry);

            return (ResponseMessages.RefreshTokenSuccessful, newAccessToken, newRefreshToken);
        }
    }
}
