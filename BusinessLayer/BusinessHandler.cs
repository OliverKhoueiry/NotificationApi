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
                return ResponseMessages.EmailAlreadyRegistered;

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
                // ✅ Send welcome email
                string subject = "Welcome to Our Service!";
                string body = $@"
                    <p>Hello {user.Username},</p>
                    <p>Thank you for registering with us. We're excited to have you onboard!</p>
                    <p>You can now login and start using all of our features.</p>
                    <br/>
                    <p>Cheers,<br/>The Team</p>
                ";

                await _emailService.SendEmailAsync(user.Email, subject, body);

                return ResponseMessages.RegistrationSuccessful;
            }

            return new ApiResponse(ResponseMessages.ErrorCode, "Registration failed.");
        }

        public async Task<(ApiResponse, string?, string?, string?)> LoginAsync(LoginRequest request)
        {
            var user = await _dataHandler.GetUserByEmailAsync(request.Email);
            if (user == null || !PasswordHasher.VerifyPassword(request.Password, user.PasswordHash))
                return (ResponseMessages.InvalidCredentials, null, null, null);

            string accessToken = _jwtService.GenerateAccessToken(user);
            string refreshToken = _jwtService.GenerateRefreshToken();
            DateTime refreshTokenExpiry = DateTime.UtcNow.AddDays(7);

            await _dataHandler.UpdateRefreshTokenAsync(user.Id, refreshToken, refreshTokenExpiry);

            return (ResponseMessages.LoginSuccessful, accessToken, refreshToken, user.Role);
        }



        public async Task<ApiResponse> ForgetPasswordAsync(string email)
        {
            var user = await _dataHandler.GetUserByEmailAsync(email);
            if (user == null)
                return ResponseMessages.UserNotFound;

            string resetToken = Guid.NewGuid().ToString();
            DateTime expiry = DateTime.UtcNow.AddMinutes(15);
            await _dataHandler.SaveResetTokenAsync(user.Id, resetToken, expiry);

            string resetLink = $"https://localhost:3000/ResetPassword?token={resetToken}";
            string subject = "Password Reset Request";
            string body = $@"
                <p>Hello {user.Username},</p>
                <p>Click below to reset your password (valid for 15 minutes):</p> 
                <a href='{resetLink}'>Reset Password</a>";

            await _emailService.SendEmailAsync(email, subject, body);

            return ResponseMessages.ForgetPasswordEmailSent;
        }

        public async Task<ApiResponse> ResetPasswordAsync(ResetPasswordRequest request)
        {
            var user = await _dataHandler.GetUserByResetTokenAsync(request.Token);
            if (user == null || user.ResetTokenExpiry < DateTime.UtcNow)
                return new ApiResponse(ResponseMessages.ErrorCode, "Invalid or expired reset token.");

            if (request.NewPassword != request.ConfirmNewPassword)
                return new ApiResponse(ResponseMessages.ValidationErrorCode, "Passwords do not match.");

            string hashedPassword = PasswordHasher.HashPassword(request.NewPassword);
            await _dataHandler.UpdatePasswordAsync(user.Id, hashedPassword);
            await _dataHandler.ClearResetTokenAsync(user.Id);

            return new ApiResponse(ResponseMessages.SuccessCode, "Password reset successful.");
        }

        public async Task<(ApiResponse, string?, string?)> RefreshTokenAsync(string refreshToken)
        {
            var user = await _dataHandler.GetUserByRefreshTokenAsync(refreshToken);
            if (user == null || user.RefreshTokenExpiry < DateTime.UtcNow)
                return (ResponseMessages.InvalidRefreshToken, null, null);

            string newAccessToken = _jwtService.GenerateAccessToken(user);
            string newRefreshToken = _jwtService.GenerateRefreshToken();
            DateTime newRefreshTokenExpiry = DateTime.UtcNow.AddDays(7);

            await _dataHandler.UpdateRefreshTokenAsync(user.Id, newRefreshToken, newRefreshTokenExpiry);

            return (ResponseMessages.RefreshTokenSuccessful, newAccessToken, newRefreshToken);
        }

        public async Task<ApiResponse> LogoutAsync(string refreshToken)
        {
            var user = await _dataHandler.GetUserByRefreshTokenAsync(refreshToken);
            if (user == null)
                return new ApiResponse(ResponseMessages.ErrorCode, "Invalid refresh token.");

            await _dataHandler.ClearRefreshTokenAsync(user.Id);
            return new ApiResponse(ResponseMessages.SuccessCode, "Logout successful.");
        }


        public async Task<IEnumerable<CourseCategory>> GetAllCategoriesAsync()
        {
            return await _dataHandler.GetAllCategoriesAsync();
        }

        public async Task<IEnumerable<Course>> GetCoursesByCategoryAsync(int categoryId)
        {
            return await _dataHandler.GetCoursesByCategoryAsync(categoryId);
        }

        public async Task<ApiResponse> AddCourseAsync(Course course)
        {
            var rowsAffected = await _dataHandler.AddCourseAsync(course);
            if (rowsAffected > 0)
                return ResponseMessages.AddCourseSuccessful;

            return ResponseMessages.AddCourseFailed;
        }

        public async Task<ApiResponse> UpdateCourseAsync(Course course)
        {
            var rowsAffected = await _dataHandler.UpdateCourseAsync(course);
            if (rowsAffected > 0)
                return ResponseMessages.UpdateCourseSuccessful;

            return ResponseMessages.UpdateCourseFailed;
        }

        public async Task<ApiResponse> DeleteCourseAsync(int courseId)
        {
            var rowsAffected = await _dataHandler.DeleteCourseAsync(courseId);

            if (rowsAffected >= 1)
            {
                return ResponseMessages.CourseDeleted;
            }

            // Return failed only if no rows affected
            return ResponseMessages.CourseDeleteFailed;
        }


        public async Task<ApiResponse> AddReviewAsync(Review review)
        {
            var rowsAffected = await _dataHandler.AddReviewAsync(review);
            return rowsAffected > 0
                ? new ApiResponse(ResponseMessages.SuccessCode, "Review added successfully.")
                : new ApiResponse(ResponseMessages.ErrorCode, "Failed to add review.");
        }

        public async Task<IEnumerable<Review>> GetReviewsByCourseAsync(int courseId)
        {
            return await _dataHandler.GetReviewsByCourseAsync(courseId);
        }

        public async Task<ApiResponse> DeleteReviewAsync(int reviewId)
        {
            var rowsAffected = await _dataHandler.DeleteReviewAsync(reviewId);
            return rowsAffected > 0
                ? new ApiResponse(ResponseMessages.SuccessCode, "Review deleted successfully.")
                : new ApiResponse(ResponseMessages.ErrorCode, "Failed to delete review.");
        }

        public async Task<ApiResponse> PromoteUserToAdminAsync(int userId)
        {
            var rowsAffected = await _dataHandler.UpdateUserRoleAsync(userId, "Admin");
            return rowsAffected > 0
                ? new ApiResponse(ResponseMessages.SuccessCode, "User promoted to Admin.")
                : new ApiResponse(ResponseMessages.ErrorCode, "Failed to promote user.");
        }


        public async Task<ApiResponse> AddCategoryAsync(CourseCategory category)
        {
            var rowsAffected = await _dataHandler.AddCategoryAsync(category);
            return rowsAffected > 0
                ? new ApiResponse(ResponseMessages.SuccessCode, "Category added successfully.")
                : new ApiResponse(ResponseMessages.ErrorCode, "Failed to add category.");
        }

        public async Task<ApiResponse> DeleteCategoryAsync(int categoryId)
        {
            var rowsAffected = await _dataHandler.DeleteCategoryAsync(categoryId);

            if (rowsAffected > 0)
                return new ApiResponse(ResponseMessages.SuccessCode, "Category deleted successfully.");

            return new ApiResponse(ResponseMessages.ErrorCode, "Failed to delete category.");
        }

    }
}
