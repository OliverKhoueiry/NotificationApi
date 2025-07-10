using CommonLayer.Common;
using CommonLayer.Models;
using DataLayer;

namespace BusinessLayer
{
    public class BusinessHandler : IBusinessHandler
    {
        private readonly IDataHandler _dataHandler;
        private readonly EmailService _emailService;
        private readonly JwtService _jwtService;

        public BusinessHandler(IDataHandler dataHandler, EmailService emailService, JwtService jwtService)
        {
            _dataHandler = dataHandler;
            _emailService = emailService;
            _jwtService = jwtService;
        }

        public async Task<ApiResponse> RegisterAsync(RegisterRequest request)
        {
            if (request.Password != request.ConfirmPassword)
                return ResponseMessages.PasswordsDoNotMatch;

            var existingUser = await _dataHandler.GetUserByEmailAsync(request.Email);
            if (existingUser != null)
                return ResponseMessages.EmailAlreadyRegistered;

            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = PasswordHasher.HashPassword(request.Password),
                CreatedAt = DateTime.UtcNow
            };

            await _dataHandler.AddUserAsync(user);
            return ResponseMessages.RegistrationSuccessful;
        }

        public async Task<(ApiResponse, User?, string?, string?)> LoginAsync(LoginRequest request)
        {
            var user = await _dataHandler.GetUserByEmailAsync(request.Email);
            if (user == null || !PasswordHasher.VerifyPassword(request.Password, user.PasswordHash))
                return (ResponseMessages.InvalidCredentials, null, null, null);

            var accessToken = _jwtService.GenerateAccessToken(user);
            var refreshToken = _jwtService.GenerateRefreshToken();
            var refreshTokenExpiry = DateTime.UtcNow.AddDays(7);

            await _dataHandler.UpdateRefreshTokenAsync(user.Id, refreshToken, refreshTokenExpiry);

            return (ResponseMessages.LoginSuccessful, user, accessToken, refreshToken);
        }

        public async Task<(ApiResponse, string?)> RefreshTokenAsync(string refreshToken)
        {
            var user = await _dataHandler.GetUserByRefreshTokenAsync(refreshToken);
            if (user == null || user.RefreshTokenExpiry < DateTime.UtcNow)
                return (ResponseMessages.InvalidRefreshToken, null);

            var newAccessToken = _jwtService.GenerateAccessToken(user);
            return (ResponseMessages.RefreshTokenSuccessful, newAccessToken);
        }

        public async Task<ApiResponse> ForgetPasswordAsync(string email)
        {
            var user = await _dataHandler.GetUserByEmailAsync(email);
            if (user == null)
                return ResponseMessages.InvalidCredentials;

            string subject = "Password Reset Request";
            string body = $@"
                <h2>Password Reset</h2>
                <p>Hello {user.Username},</p>
                <p>This is a test email for password reset.</p>
                <p>Click <a href='https://example.com/reset-password'>here</a> to reset your password.</p>";

            await _emailService.SendEmailAsync(email, subject, body);

            return ResponseMessages.ForgetPasswordEmailSent;
        }

        Task<(ApiResponse, string?, string?)> IBusinessHandler.LoginAsync(LoginRequest request)
        {
            throw new NotImplementedException();
        }

        Task<(ApiResponse, string?, string?)> IBusinessHandler.RefreshTokenAsync(string refreshToken)
        {
            throw new NotImplementedException();
        }
    }
}
