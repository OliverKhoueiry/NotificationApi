using CommonLayer.Common;
using CommonLayer.Models;

namespace BusinessLayer
{
    public interface IBusinessHandler
    {
        Task<ApiResponse> RegisterAsync(RegisterRequest request);
        Task<(ApiResponse, string?, string?)> LoginAsync(LoginRequest request);
        Task<ApiResponse> ForgetPasswordAsync(string email);
        Task<(ApiResponse, string?, string?)> RefreshTokenAsync(string refreshToken);
        Task<ApiResponse> ResetPasswordAsync(ResetPasswordRequest request);
        Task<ApiResponse> LogoutAsync(string refreshToken);
    }
}
