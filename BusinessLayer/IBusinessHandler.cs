using CommonLayer.Models;

namespace BusinessLayer
{
    public interface IBusinessHandler
    {
        Task<string> RegisterAsync(RegisterRequest request);
        Task<User?> LoginAsync(LoginRequest request);
    }
}
