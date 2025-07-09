using CommonLayer.Common;
using CommonLayer.Models;
using DataLayer;


namespace BusinessLayer
{
    public class BusinessHandler : IBusinessHandler
    {
        private readonly IDataHandler _dataHandler;
        private readonly EmailService _emailService;

        public BusinessHandler(IDataHandler dataHandler, EmailService emailService)
        {
            _dataHandler = dataHandler;
            _emailService = emailService;
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
                PasswordHash = PasswordHasher.HashPassword(request.Password)
            };

            await _dataHandler.AddUserAsync(user);
            return ResponseMessages.RegistrationSuccessful;
        }

        public async Task<(ApiResponse, User?)> LoginAsync(LoginRequest request)
        {
            var user = await _dataHandler.GetUserByEmailAsync(request.Email);
            if (user == null || !PasswordHasher.VerifyPassword(request.Password, user.PasswordHash))
                return (ResponseMessages.InvalidCredentials, null);

            return (ResponseMessages.LoginSuccessful, user);
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
    }
}
