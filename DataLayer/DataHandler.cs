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
                    Role = user.Role,                        // 👈 Add this
                    RefreshToken = user.RefreshToken,
                    RefreshTokenExpiry = user.RefreshTokenExpiry,
                    CreatedAt = user.CreatedAt               // 👈 Add this
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
                "ClearRefreshToken", 
                new { UserId = userId },
                commandType: CommandType.StoredProcedure);
        }


        public async Task<IEnumerable<CourseCategory>> GetAllCategoriesAsync()
        {
            using var connection = GetConnection();
            return await connection.QueryAsync<CourseCategory>(
                "GetAllCategoriesWithCourseCount",
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Course>> GetCoursesByCategoryAsync(int categoryId)
        {
            using var connection = GetConnection();
            return await connection.QueryAsync<Course>(
                "GetCoursesByCategory",
                new { CategoryId = categoryId },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> AddCourseAsync(Course course)
        {
            using var connection = GetConnection();
            return await connection.ExecuteAsync(
                "AddCourse",
                new
                {
                    course.Title,
                    course.Overview,
                    course.Price,
                    course.Level,
                    course.DurationWeeks,
                    course.OnlineClasses,
                    course.Lessons,
                    course.Quizzes,
                    course.PassPercentage,
                    course.Certificate,
                    course.Language,
                    course.CategoryId
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> UpdateCourseAsync(Course course)
        {
            using var connection = GetConnection();
            return await connection.ExecuteAsync(
                "UpdateCourse",
                new
                {
                    course.Id,
                    course.Title,
                    course.Overview,
                    course.Price,
                    course.Level,
                    course.DurationWeeks,
                    course.OnlineClasses,
                    course.Lessons,
                    course.Quizzes,
                    course.PassPercentage,
                    course.Certificate,
                    course.Language,
                    course.CategoryId
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> DeleteCourseAsync(int courseId)
        {
            using var connection = GetConnection();
            return await connection.ExecuteAsync(
                "DeleteCourse",
                new { Id = courseId },
                commandType: CommandType.StoredProcedure);
        }


        public async Task<int> AddReviewAsync(Review review)
        {
            using var connection = GetConnection();
            return await connection.ExecuteAsync(
                "AddReview",
                new
                {
                    review.CourseId,
                    review.Name,
                    review.Email,
                    review.ReviewComment,
                    review.StarsOfTheReview
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Review>> GetReviewsByCourseAsync(int courseId)
        {
            using var connection = GetConnection();
            return await connection.QueryAsync<Review>(
                "GetReviewsByCourse",
                new { CourseId = courseId },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> DeleteReviewAsync(int reviewId)
        {
            using var connection = GetConnection();
            return await connection.ExecuteAsync(
                "DeleteReview",
                new { ReviewId = reviewId },
                commandType: CommandType.StoredProcedure);
        }

        private IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public async Task<int> UpdateUserRoleAsync(int userId, string role)
        {
            using var connection = CreateConnection();
            var parameters = new { UserId = userId, Role = role };

            var rowsAffected = await connection.ExecuteAsync(
                "UpdateUserRole", 
                parameters,
                commandType: System.Data.CommandType.StoredProcedure
            );

            return rowsAffected;
        }


        public async Task<int> AddCategoryAsync(CourseCategory category)
        {
            using var connection = GetConnection();
            return await connection.ExecuteAsync(
                "AddCategory",
                new { Name = category.Name },
                commandType: CommandType.StoredProcedure);
        }




    }
}
