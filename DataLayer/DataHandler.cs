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
            var parameters = new DynamicParameters();
            parameters.Add("@Email", email);

            return await connection.QueryFirstOrDefaultAsync<User>(
                "GetUserByEmail",
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> AddUserAsync(User user)
        {
            using var connection = new SqlConnection(_connectionString);
            var parameters = new DynamicParameters();
            parameters.Add("@Username", user.Username);
            parameters.Add("@Email", user.Email);
            parameters.Add("@PasswordHash", user.PasswordHash);

            return await connection.ExecuteAsync(
                "AddUser",
                parameters,
                commandType: CommandType.StoredProcedure);
        }
    }
}
