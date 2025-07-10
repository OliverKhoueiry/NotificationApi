using CommonLayer.Models;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace DataLayer
{
    public class DataHandler : IDataHandler
    {
        private readonly string _connectionString;

        public DataHandler(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryFirstOrDefaultAsync<User>(
                "GetUserByEmail",
                new { Email = email },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<User?> GetUserByRefreshTokenAsync(string refreshToken)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryFirstOrDefaultAsync<User>(
                "GetUserByRefreshToken",
                new { RefreshToken = refreshToken },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> AddUserAsync(User user)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.ExecuteAsync(
                "AddUser",
                new
                {
                    Username = user.Username,
                    Email = user.Email,
                    PasswordHash = user.PasswordHash,
                    RefreshToken = user.RefreshToken,                // can be null at registration
                    RefreshTokenExpiry = user.RefreshTokenExpiry     // can be null at registration
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task UpdateRefreshTokenAsync(int userId, string refreshToken, DateTime refreshTokenExpiry)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.ExecuteAsync(
                "UpdateRefreshToken",
                new
                {
                    UserId = userId,
                    RefreshToken = refreshToken,
                    RefreshTokenExpiry = refreshTokenExpiry
                },
                commandType: CommandType.StoredProcedure);
        }
    }
}
