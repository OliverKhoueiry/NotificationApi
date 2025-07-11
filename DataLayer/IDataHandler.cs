using CommonLayer.Models;

namespace DataLayer
{
    public interface IDataHandler
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserByRefreshTokenAsync(string refreshToken);
        Task<int> AddUserAsync(User user);
        Task UpdateRefreshTokenAsync(int userId, string refreshToken, DateTime refreshTokenExpiry);
        Task SaveResetTokenAsync(int userId, string token, DateTime expiry);
        Task<User?> GetUserByResetTokenAsync(string token);
        Task UpdateUserPasswordAsync(int userId, string hashedPassword);
        Task ClearResetTokenAsync(int userId);

    }
}
