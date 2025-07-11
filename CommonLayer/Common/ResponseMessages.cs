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
        public static ApiResponse LogoutSuccessful = new ApiResponse(SuccessCode, "Logout successful.");


    }

    public record ApiResponse(int Code, string Description);
}
