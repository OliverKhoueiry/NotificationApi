namespace CommonLayer.Common
{
    public static class ResponseMessages
    {
        public const int SuccessCode = 0;
        public const int ErrorCode = 1;
        public const int AlreadyExistsCode = 2;
        public const int ValidationErrorCode = 3;

        public static readonly ApiResponse RegistrationSuccessful =
            new ApiResponse(SuccessCode, "Registration successful.");

        public static readonly ApiResponse PasswordsDoNotMatch =
            new ApiResponse(ValidationErrorCode, "Passwords do not match.");

        public static readonly ApiResponse EmailAlreadyRegistered =
            new ApiResponse(AlreadyExistsCode, "Email already registered.");

        public static readonly ApiResponse InvalidCredentials =
            new ApiResponse(ErrorCode, "Invalid credentials.");

        public static readonly ApiResponse ForgetPasswordEmailSent =
            new ApiResponse(SuccessCode, "Password reset email sent.");

        public static readonly ApiResponse UserNotFound =
            new ApiResponse(ErrorCode, "This email is not registered.");

        public static readonly ApiResponse LoginSuccessful =
            new ApiResponse(SuccessCode, "Login successful.");

        public static readonly ApiResponse InvalidRefreshToken =
            new ApiResponse(ErrorCode, "Invalid or expired refresh token.");

        public static readonly ApiResponse RefreshTokenSuccessful =
            new ApiResponse(SuccessCode, "New access token generated successfully.");

        public static readonly ApiResponse PasswordResetSuccessful =
            new ApiResponse(SuccessCode, "Password reset successfully.");

        public static readonly ApiResponse LogoutSuccessful =
            new ApiResponse(SuccessCode, "Logout successful.");

        //  Course-related responses
        public static readonly ApiResponse AddCourseSuccessful =
            new ApiResponse(SuccessCode, "Course added successfully.");

        public static readonly ApiResponse AddCourseFailed =
            new ApiResponse(ErrorCode, "Failed to add course.");

        public static readonly ApiResponse UpdateCourseSuccessful =
            new ApiResponse(SuccessCode, "Course updated successfully.");

        public static readonly ApiResponse UpdateCourseFailed =
            new ApiResponse(ErrorCode, "Failed to update course.");

        public static readonly ApiResponse CourseDeleted =
            new ApiResponse(SuccessCode, "Course deleted successfully.");

        public static readonly ApiResponse CourseDeleteFailed =
            new ApiResponse(ErrorCode, "Failed to delete course.");

        public static readonly ApiResponse SessionAddedSuccessfully =
            new ApiResponse(SuccessCode, "Session added successfully.");

        public static readonly ApiResponse CategoryUpdatedSuccessfully =
            new ApiResponse(SuccessCode, "Category updated successfully");

        public static readonly ApiResponse CategoryUpdateFailed =
            new ApiResponse(ErrorCode, "Failed to update category");


    }

    public record ApiResponse(int Code, string Description);
}
