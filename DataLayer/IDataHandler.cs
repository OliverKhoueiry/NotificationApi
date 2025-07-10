using CommonLayer.Models;

namespace DataLayer
{
    public interface IDataHandler
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserByRefreshTokenAsync(string refreshToken);
        Task<int> AddUserAsync(User user);
        Task UpdateRefreshTokenAsync(int userId, string refreshToken, DateTime refreshTokenExpiry);
    }
}
