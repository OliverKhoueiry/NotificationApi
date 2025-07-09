using CommonLayer.Common;
using CommonLayer.Models;

namespace BusinessLayer
{
    public interface IBusinessHandler
    {
        Task<ApiResponse> RegisterAsync(RegisterRequest request);
        Task<(ApiResponse, User?)> LoginAsync(LoginRequest request);
        Task<ApiResponse> ForgetPasswordAsync(string email);
    }
}
