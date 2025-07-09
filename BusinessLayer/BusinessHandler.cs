using CommonLayer.Common;
using CommonLayer.Models;
using DataLayer;

namespace BusinessLayer
{
    public class BusinessHandler : IBusinessHandler
    {
        private readonly IDataHandler _dataHandler;

        public BusinessHandler(IDataHandler dataHandler)
        {
            _dataHandler = dataHandler;
        }

        public async Task<string> RegisterAsync(RegisterRequest request)
        {
            if (request.Password != request.ConfirmPassword)
                return "Passwords do not match.";

            var existingUser = await _dataHandler.GetUserByEmailAsync(request.Email);
            if (existingUser != null)
                return "Email already registered.";

            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = PasswordHasher.HashPassword(request.Password)
            };

            await _dataHandler.AddUserAsync(user);
            return "Registration successful.";
        }

        public async Task<User?> LoginAsync(LoginRequest request)
        {
            var user = await _dataHandler.GetUserByEmailAsync(request.Email);
            if (user == null)
                return null;


            var hashed = PasswordHasher.HashPassword(request.Password);
            return user.PasswordHash == hashed ? user : null;
        }
    }
}
