using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLayer.Common;
using CommonLayer.Dtos;
using CommonLayer.Models;
using DataLayer;
using static System.Collections.Specialized.BitVector32;
using CommonLayer.Dtos;


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
                string subject = "Welcome to Our Service!";
                string body = $@"
                    <p>Hello {user.Username},</p>
                    <p>Thank you for registering with us. We're excited to have you onboard!</p>
                    <p>You can now login and start using all of our features.</p>
                    <br/>
                    <p>Cheers,<br/>The Team</p>";

                await _emailService.SendEmailAsync(user.Email, subject, body);

                return ResponseMessages.RegistrationSuccessful;
            }

            return new ApiResponse(ResponseMessages.ErrorCode, "Registration failed.");
        }

        public async Task<(ApiResponse, string?, string?, List<UserPermission>)> LoginAsync(LoginRequest request)
        {
            var user = await _dataHandler.GetUserByEmailAsync(request.Email);
            if (user == null || !PasswordHasher.VerifyPassword(request.Password, user.PasswordHash))
                return (ResponseMessages.InvalidCredentials, null, null, new List<UserPermission>());

            user.Permissions = await _dataHandler.GetUserPermissionsAsync(user.Id);

            string accessToken = _jwtService.GenerateAccessToken(user);
            string refreshToken = _jwtService.GenerateRefreshToken();
            DateTime refreshTokenExpiry = DateTime.UtcNow.AddDays(7);

            await _dataHandler.UpdateRefreshTokenAsync(user.Id, refreshToken, refreshTokenExpiry);

            return (ResponseMessages.LoginSuccessful, accessToken, refreshToken, user.Permissions);
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

        public async Task<List<UserPermission>> GetAllSectionsAsync()
        {
            return await _dataHandler.GetAllSectionsAndActionsAsync();
        }

        public Task<ApiResponse> AddWebRoleAsync(string name)
        {
            return ExecuteSafeAsync(() => _dataHandler.AddWebRoleAsync(name), "Role added successfully.");
        }

        public Task<ApiResponse> UpdateWebRoleAsync(int id, string name)
        {
            return ExecuteSafeAsync(() => _dataHandler.UpdateWebRoleAsync(id, name), "Role updated successfully.");
        }

        public Task<ApiResponse> DeleteWebRoleAsync(int id)
        {
            return ExecuteSafeAsync(() => _dataHandler.DeleteWebRoleAsync(id), "Role deleted successfully.");
        }

        public Task<List<WebRole>> GetWebRolesAsync()
        {
            return _dataHandler.GetWebRolesAsync();
        }

        public Task<ApiResponse> AssignPermissionAsync(int roleId, int sectionId, string action)
        {
            return ExecuteSafeAsync(() => _dataHandler.AssignPermissionAsync(roleId, sectionId, action), "Permission assigned successfully.");
        }

        public Task<List<UserPermission>> GetRolePermissionsAsync(int roleId)
        {
            return _dataHandler.GetRolePermissionsAsync(roleId);
        }

        private async Task<ApiResponse> ExecuteSafeAsync(Func<Task> action, string successMessage)
        {
            try
            {
                await action();
                return new ApiResponse(ResponseMessages.SuccessCode, successMessage);
            }
            catch (Exception ex)
            {
                return new ApiResponse(ResponseMessages.ErrorCode, ex.Message);
            }
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
            try
            {
                await _dataHandler.AddCourseAsync(course);
                return new ApiResponse(ResponseMessages.SuccessCode, "Course added successfully.");
            }
            catch (Exception ex)
            {
                return new ApiResponse(ResponseMessages.ErrorCode, ex.Message);
            }
        }

        public async Task<ApiResponse> UpdateCourseAsync(Course course)
        {
            try
            {
                await _dataHandler.UpdateCourseAsync(course);
                return new ApiResponse(ResponseMessages.SuccessCode, "Course updated successfully.");
            }
            catch (Exception ex)
            {
                return new ApiResponse(ResponseMessages.ErrorCode, ex.Message);
            }
        }

        public async Task<ApiResponse> DeleteCourseAsync(int courseId)
        {
            try
            {
                await _dataHandler.DeleteCourseAsync(courseId);
                return new ApiResponse(ResponseMessages.SuccessCode, "Course deleted successfully.");
            }
            catch (Exception ex)
            {
                return new ApiResponse(ResponseMessages.ErrorCode, ex.Message);
            }
        }

        public async Task<ApiResponse> DeleteReviewAsync(int reviewId)
        {
            try
            {
                await _dataHandler.DeleteReviewAsync(reviewId);
                return new ApiResponse(ResponseMessages.SuccessCode, "Review deleted successfully.");
            }
            catch (Exception ex)
            {
                return new ApiResponse(ResponseMessages.ErrorCode, ex.Message);
            }
        }

        public async Task<ApiResponse> PromoteUserToRoleAsync(int userId, string roleName)
        {
            try
            {
                await _dataHandler.PromoteUserToRoleAsync(userId, roleName);
                return new ApiResponse(ResponseMessages.SuccessCode, "User promoted successfully.");
            }
            catch (Exception ex)
            {
                return new ApiResponse(ResponseMessages.ErrorCode, $"Error promoting user: {ex.Message}");
            }
        }


        public async Task<ApiResponse> AddCategoryAsync(CourseCategory category)
        {
            try
            {
                await _dataHandler.AddCategoryAsync(category);
                return new ApiResponse(ResponseMessages.SuccessCode, "Category added successfully.");
            }
            catch (Exception ex)
            {
                return new ApiResponse(ResponseMessages.ErrorCode, ex.Message);
            }
        }

        public async Task<ApiResponse> DeleteCategoryAsync(int categoryId)
        {
            try
            {
                await _dataHandler.DeleteCategoryAsync(categoryId);
                return new ApiResponse(ResponseMessages.SuccessCode, "Category deleted successfully.");
            }
            catch (Exception ex)
            {
                return new ApiResponse(ResponseMessages.ErrorCode, ex.Message);
            }
        }

       

        public async Task<ApiResponse> AddReviewAsync(Review review)
        {
            try
            {
                review.ReviewDate = DateTime.UtcNow; // Set current date
                await _dataHandler.AddReviewAsync(review);
                return new ApiResponse(ResponseMessages.SuccessCode, "Review added successfully.");
            }
            catch (Exception ex)
            {
                return new ApiResponse(ResponseMessages.ErrorCode, ex.Message);
            }
        }



        public async Task<IEnumerable<Review>> GetReviewsByCourseAsync(int courseId)
        {
            return await _dataHandler.GetReviewsByCourseAsync(courseId);
        }


        public async Task<ApiResponse> AddRoleSectionAsync(RoleSection roleSection)
        {
            try
            {
                await _dataHandler.AddRoleSectionAsync(roleSection);
                return new ApiResponse(ResponseMessages.SuccessCode, "Role section added successfully.");
            }
            catch (Exception ex)
            {
                return new ApiResponse(ResponseMessages.ErrorCode, ex.Message);
            }
        }

        public async Task<ApiResponse> UpdateRoleSectionAsync(RoleSection roleSection)
        {
            try
            {
                await _dataHandler.UpdateRoleSectionAsync(roleSection);
                return new ApiResponse(ResponseMessages.SuccessCode, "Role section updated successfully.");
            }
            catch (Exception ex)
            {
                return new ApiResponse(ResponseMessages.ErrorCode, ex.Message);
            }
        }

        public async Task<ApiResponse> DeleteRoleSectionAsync(int roleSectionId)
        {
            try
            {
                await _dataHandler.DeleteRoleSectionAsync(roleSectionId);
                return new ApiResponse(ResponseMessages.SuccessCode, "Role section deleted successfully.");
            }
            catch (Exception ex)
            {
                return new ApiResponse(ResponseMessages.ErrorCode, ex.Message);
            }
        }

        public async Task<List<RoleSection>> GetRoleSectionsAsync(int roleId)
        {
            return await _dataHandler.GetRoleSectionsAsync(roleId);
        }

        public async Task<RoleSection?> GetRoleSectionByNameAsync(string roleName, string sectionName)
        {
            return await _dataHandler.GetRoleSectionByNameAsync(roleName, sectionName);
        }


        public async Task<IEnumerable<Review>> GetAllReviewsAsync()
        {
            return await _dataHandler.GetAllReviewsAsync();
        }



        public async Task<IEnumerable<Course>> GetAllCoursesAsync()
        {
            return await _dataHandler.GetAllCoursesAsync();
        }


        public async Task<ApiResponse> AddSectionAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return new ApiResponse(ResponseMessages.ValidationErrorCode, "Section name is required.");

            return await _dataHandler.AddSectionAsync(name);
        }

        public async Task<ApiResponse> UpdateSectionAsync(int id, string name)
        {
            if (id <= 0 || string.IsNullOrWhiteSpace(name))
                return new ApiResponse(ResponseMessages.ValidationErrorCode, "Valid section id and name are required.");

            return await _dataHandler.UpdateSectionAsync(id, name);
        }

        public async Task<ApiResponse> DeleteSectionAsync(int id)
        {
            if (id <= 0)
                return new ApiResponse(ResponseMessages.ValidationErrorCode, "Valid section id is required.");

            return await _dataHandler.DeleteSectionAsync(id);
        }


        public async Task<ApiResponse> AddSessionAsync(Session session)
        {
            await _dataHandler.AddSessionAsync(session);
            return ResponseMessages.SessionAddedSuccessfully;
        }



        public async Task<ApiResponse> UpdateSessionAsync(Session session)
            => await _dataHandler.UpdateSessionAsync(session);

        public async Task<ApiResponse> DeleteSessionAsync(int sessionId)
            => await _dataHandler.DeleteSessionAsync(sessionId);

        public async Task<IEnumerable<Session>> GetAllSessionsAsync()
            => await _dataHandler.GetAllSessionsAsync();

        public async Task<ApiResponse> AddSessionVideoAsync(SessionVideo video)
            => await _dataHandler.AddSessionVideoAsync(video);

        public async Task<ApiResponse> DeleteSessionVideoAsync(int videoId)
            => await _dataHandler.DeleteSessionVideoAsync(videoId);

        public async Task<IEnumerable<SessionVideo>> GetSessionVideosAsync(int sessionId)
            => await _dataHandler.GetSessionVideosAsync(sessionId);

        public async Task<List<CategoryDto>> LoadCategoriesAsync()
        {
            return await _dataHandler.LoadCategoriesAsync();
        }

        public async Task<ApiResponse> UpdateCategoryAsync(CategoryDto category)
        {
            return await _dataHandler.UpdateCategoryAsync(category);
        }

        public async Task<Course?> GetCourseByIdAsync(int courseId)
        {
            return await _dataHandler.GetCourseByIdAsync(courseId);
        }


        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            return await _dataHandler.GetAllUsersAsync();
        }
        public async Task<ApiResponse> AddUserAsync(UserDto userDto)
        {
            return await _dataHandler.AddUserAsync(userDto);
        }

        public async Task<ApiResponse> UpdateUserAsync(int userId, UpdateUserDto userDto)
        {
            return await _dataHandler.UpdateUserAsync(userId, userDto);
        }

        public async Task<ApiResponse> DeleteUserAsync(int userId)
        {
            return await _dataHandler.DeleteUserAsync(userId);
        }
        public async Task<ApiResponse> AddRoleAsync(AddRoleDto roleDto)
        {
            return await _dataHandler.AddRoleAsync(roleDto);
        }
        public async Task<ApiResponse> UpdateRoleAsync(int roleId, AddRoleDto roleDto)
        {
            return await _dataHandler.UpdateRoleAsync(roleId, roleDto);
        }
        public async Task<HomeResponseDto> GetHomeDataAsync()
        {
            return await _dataHandler.GetHomeDataAsync();
        }

    }
}
