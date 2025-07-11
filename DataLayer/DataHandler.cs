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

        private SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            using var connection = GetConnection();
            return await connection.QueryFirstOrDefaultAsync<User>(
                "GetUserByEmail",
                new { Email = email },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<User?> GetUserByRefreshTokenAsync(string refreshToken)
        {
            using var connection = GetConnection();
            return await connection.QueryFirstOrDefaultAsync<User>(
                "GetUserByRefreshToken",
                new { RefreshToken = refreshToken },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> AddUserAsync(User user)
        {
            using var connection = GetConnection();
            return await connection.ExecuteAsync(
                "AddUser",
                new
                {
                    Username = user.Username,
                    Email = user.Email,
                    PasswordHash = user.PasswordHash,
                    RefreshToken = user.RefreshToken,
                    RefreshTokenExpiry = user.RefreshTokenExpiry
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task UpdateRefreshTokenAsync(int userId, string refreshToken, DateTime refreshTokenExpiry)
        {
            using var connection = GetConnection();
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

        public async Task SaveResetTokenAsync(int userId, string token, DateTime expiry)
        {
            using var connection = GetConnection();
            await connection.ExecuteAsync(
                "SaveResetToken",
                new { UserId = userId, Token = token, Expiry = expiry },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<User?> GetUserByResetTokenAsync(string token)
        {
            using var connection = GetConnection();
            return await connection.QueryFirstOrDefaultAsync<User>(
                "GetUserByResetToken",
                new { ResetToken = token },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> UpdatePasswordAsync(int userId, string newHashedPassword)
        {
            using var connection = GetConnection();
            return await connection.ExecuteAsync(
                "UpdateUserPassword",
                new
                {
                    UserId = userId,
                    PasswordHash = newHashedPassword
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task ClearResetTokenAsync(int userId)
        {
            using var connection = GetConnection();
            await connection.ExecuteAsync(
                "ClearResetToken",
                new { UserId = userId },
                commandType: CommandType.StoredProcedure);
        }

        public Task UpdateUserPasswordAsync(int userId, string hashedPassword)
        {
            throw new NotImplementedException();
        }
        public async Task ClearRefreshTokenAsync(int userId)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.ExecuteAsync(
                "ClearRefreshToken", // Your stored procedure name
                new { UserId = userId },
                commandType: CommandType.StoredProcedure);
        }

    }
}
