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

        public async Task<int> AddUserAsync(User user)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.ExecuteAsync(
                "AddUser",
                new
                {
                    Username = user.Username,
                    Email = user.Email,
                    PasswordHash = user.PasswordHash
                },
                commandType: CommandType.StoredProcedure);
        }
    }
}
