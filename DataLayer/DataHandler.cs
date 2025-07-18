﻿using CommonLayer.Models;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLayer.Common;
using static System.Collections.Specialized.BitVector32;

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
            var user = await connection.QueryFirstOrDefaultAsync<User>(
                "GetUserByEmail",
                new { Email = email },
                commandType: CommandType.StoredProcedure);

            if (user != null)
            {
                user.Permissions = await GetUserPermissionsAsync(user.Id);
            }

            return user;
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
                    Role = user.Role,
                    RefreshToken = user.RefreshToken,
                    RefreshTokenExpiry = user.RefreshTokenExpiry,
                    CreatedAt = user.CreatedAt
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

        public async Task ClearRefreshTokenAsync(int userId)
        {
            using var connection = GetConnection();
            await connection.ExecuteAsync(
                "ClearRefreshToken",
                new { UserId = userId },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<List<UserPermission>> GetUserPermissionsAsync(int userId)
        {
            using var connection = GetConnection();
            var permissions = await connection.QueryAsync<UserPermission>(
                "GetUserPermissions",
                new { UserId = userId },
                commandType: CommandType.StoredProcedure);
            return permissions.ToList();
        }

        public async Task<List<UserPermission>> GetAllSectionsAndActionsAsync()
        {
            using var connection = GetConnection();
            var permissions = await connection.QueryAsync<UserPermission>(
                "GetAllSectionsAndActions",
                commandType: CommandType.StoredProcedure);
            return permissions.ToList();
        }

        public async Task AddWebRoleAsync(string name)
        {
            using var connection = GetConnection();
            await connection.ExecuteAsync(
                "AddWebRole",
                new { Name = name },
                commandType: CommandType.StoredProcedure);
        }

        public async Task UpdateWebRoleAsync(int id, string name)
        {
            using var connection = GetConnection();
            await connection.ExecuteAsync(
                "UpdateWebRole",
                new { Id = id, Name = name },
                commandType: CommandType.StoredProcedure);
        }

        public async Task DeleteWebRoleAsync(int id)
        {
            using var connection = GetConnection();
            await connection.ExecuteAsync(
                "DeleteWebRole",
                new { Id = id },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<List<WebRole>> GetWebRolesAsync()
        {
            using var connection = GetConnection();
            var roles = await connection.QueryAsync<WebRole>(
                "GetWebRoles",
                commandType: CommandType.StoredProcedure);
            return roles.ToList();
        }

        public async Task AssignPermissionAsync(int roleId, int sectionId, string action)
        {
            using var connection = GetConnection();
            await connection.ExecuteAsync(
                "AssignPermissionsToRole",
                new { RoleId = roleId, SectionId = sectionId, Action = action },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<List<UserPermission>> GetRolePermissionsAsync(int roleId)
        {
            using var connection = GetConnection();
            var permissions = await connection.QueryAsync<UserPermission>(
                "GetRolePermissions",
                new { RoleId = roleId },
                commandType: CommandType.StoredProcedure);
            return permissions.ToList();
        }

        async Task IDataHandler.UpdatePasswordAsync(int userId, string hashedPassword)
        {
            await UpdatePasswordAsync(userId, hashedPassword);
        }


        public async Task AddRoleSectionAsync(RoleSection roleSection)
        {
            using var connection = GetConnection();
            await connection.ExecuteAsync(
                "AddRoleSection",
                new
                {
                    IdWebRole = roleSection.IdWebRole,
                    IdSection = roleSection.IdSection,
                    IsView = roleSection.IsView,
                    IsAdd = roleSection.IsAdd,
                    IsUpdate = roleSection.IsUpdate,
                    IsDelete = roleSection.IsDelete
                },
                commandType: CommandType.StoredProcedure);
        }


        public async Task UpdateRoleSectionAsync(RoleSection roleSection)
        {
            using var connection = GetConnection();
            await connection.ExecuteAsync(
                "UpdateRoleSection",
                new
                {
                    RoleSectionId = roleSection.Id,
                    IsView = roleSection.IsView,
                    IsAdd = roleSection.IsAdd,
                    IsUpdate = roleSection.IsUpdate,
                    IsDelete = roleSection.IsDelete
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task DeleteRoleSectionAsync(int roleSectionId)
        {
            using var connection = GetConnection();
            await connection.ExecuteAsync(
                "DeleteRoleSection",
                new { Id = roleSectionId },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<List<RoleSection>> GetRoleSectionsAsync(int roleId)
        {
            using var connection = GetConnection();
            var roleSections = await connection.QueryAsync<RoleSection>(
                "GetRoleSections",
                new { IdWebRole = roleId },
                commandType: CommandType.StoredProcedure);
            return roleSections.ToList();
        }

        public async Task<RoleSection?> GetRoleSectionByNameAsync(string roleName, string sectionName)
        {
            using var connection = GetConnection();
            return await connection.QueryFirstOrDefaultAsync<RoleSection>(
                "GetRoleSectionByName",
                new { RoleName = roleName, SectionName = sectionName },
                commandType: CommandType.StoredProcedure);
        }
        public async Task AddReviewAsync(Review review)
        {
            using var connection = GetConnection();
            await connection.ExecuteAsync(
                "AddReview", // Stored Procedure Name
                new
                {
                    CourseId = review.CourseId,
                    Name = review.Name,
                    Email = review.Email,
                    ReviewComment = review.ReviewComment,
                    StarsOfTheReview = review.StarsOfTheReview,
                    ReviewDate = review.ReviewDate
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Review>> GetReviewsByCourseAsync(int courseId)
        {
            using var connection = GetConnection();
            var reviews = await connection.QueryAsync<Review>(
                "GetReviewsByCourse", // Stored Procedure Name
                new { CourseId = courseId },
                commandType: CommandType.StoredProcedure);
            return reviews;
        }

        public async Task<IEnumerable<CourseCategory>> GetAllCategoriesAsync()
        {
            var categories = new List<CourseCategory>();

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("GetAllCategoriesWithCourseCount", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        categories.Add(new CourseCategory
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            CourseCount = reader.GetInt32(reader.GetOrdinal("CourseCount"))
                        });
                    }
                }
            }

            return categories;
        }

        public async Task<IEnumerable<Course>> GetCoursesByCategoryAsync(int categoryId)
        {
            var courses = new List<Course>();

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("GetCoursesByCategory", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CategoryId", categoryId);

                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        courses.Add(new Course
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Title = reader.GetString(reader.GetOrdinal("Title")),
                            Overview = reader.GetString(reader.GetOrdinal("Overview")),
                            Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                            Level = reader.GetString(reader.GetOrdinal("Level")),
                            DurationWeeks = reader.GetInt32(reader.GetOrdinal("DurationWeeks")),
                            OnlineClasses = reader.GetInt32(reader.GetOrdinal("OnlineClasses")),
                            Lessons = reader.GetInt32(reader.GetOrdinal("Lessons")),
                            Quizzes = reader.GetInt32(reader.GetOrdinal("Quizzes")),
                            PassPercentage = reader.GetInt32(reader.GetOrdinal("PassPercentage")),
                            Certificate = reader.GetBoolean(reader.GetOrdinal("Certificate")),
                            Language = reader.GetString(reader.GetOrdinal("Language")),
                            CategoryId = reader.GetInt32(reader.GetOrdinal("CategoryId"))
                        });
                    }
                }
            }

            return courses;
        }


        public async Task AddCourseAsync(Course course)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("AddCourse", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Title", course.Title);
                command.Parameters.AddWithValue("@Overview", course.Overview);
                command.Parameters.AddWithValue("@Price", course.Price);
                command.Parameters.AddWithValue("@Level", course.Level);
                command.Parameters.AddWithValue("@DurationWeeks", course.DurationWeeks);
                command.Parameters.AddWithValue("@OnlineClasses", course.OnlineClasses);
                command.Parameters.AddWithValue("@Lessons", course.Lessons);
                command.Parameters.AddWithValue("@Quizzes", course.Quizzes);
                command.Parameters.AddWithValue("@PassPercentage", course.PassPercentage);
                command.Parameters.AddWithValue("@Certificate", course.Certificate);
                command.Parameters.AddWithValue("@Language", course.Language);
                command.Parameters.AddWithValue("@CategoryId", course.CategoryId);

                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
        }


        public async Task UpdateCourseAsync(Course course)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("UpdateCourse", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Id", course.Id);
                command.Parameters.AddWithValue("@Title", course.Title);
                command.Parameters.AddWithValue("@Overview", course.Overview);
                command.Parameters.AddWithValue("@Price", course.Price);
                command.Parameters.AddWithValue("@Level", course.Level);
                command.Parameters.AddWithValue("@DurationWeeks", course.DurationWeeks);
                command.Parameters.AddWithValue("@OnlineClasses", course.OnlineClasses);
                command.Parameters.AddWithValue("@Lessons", course.Lessons);
                command.Parameters.AddWithValue("@Quizzes", course.Quizzes);
                command.Parameters.AddWithValue("@PassPercentage", course.PassPercentage);
                command.Parameters.AddWithValue("@Certificate", course.Certificate);
                command.Parameters.AddWithValue("@Language", course.Language);
                command.Parameters.AddWithValue("@CategoryId", course.CategoryId);

                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
        }


        public async Task DeleteCourseAsync(int courseId)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("DeleteCourse", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Id", courseId);

                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task DeleteReviewAsync(int reviewId)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("DeleteReview", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ReviewId", reviewId);

                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task PromoteUserToRoleAsync(int userId, string roleName)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("PromoteUserToRole", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@RoleName", roleName);

                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
        }



        public async Task AddCategoryAsync(CourseCategory category)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("AddCategory", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Name", category.Name);

                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task<ApiResponse> DeleteCategoryAsync(int categoryId)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    using (var command = new SqlCommand("DeleteCategory", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // 👇 Send the parameter that your SP expects
                        command.Parameters.AddWithValue("@Id", categoryId);

                        await connection.OpenAsync();
                        int rowsAffected = await command.ExecuteNonQueryAsync();

                        if (rowsAffected > 0)
                            return new ApiResponse(ResponseMessages.SuccessCode, "Category deleted successfully.");
                        else
                            return new ApiResponse(ResponseMessages.ErrorCode, "Category not found.");
                    }
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse(ResponseMessages.ErrorCode, $"Error: {ex.Message}");
            }
        }

        public async Task<IEnumerable<Review>> GetAllReviewsAsync()
        {
            const string spName = "GetAllReviews";

            using var connection = new SqlConnection(_connectionString);
            var reviews = await connection.QueryAsync<Review>(
                spName,
                commandType: CommandType.StoredProcedure);

            return reviews;
        }

        public async Task<IEnumerable<Course>> GetAllCoursesAsync()
        {
            const string spName = "GetAllCourses";

            using var connection = new SqlConnection(_connectionString);
            var courses = await connection.QueryAsync<Course>(
                spName,
                commandType: CommandType.StoredProcedure);

            return courses;
        }

        public async Task<ApiResponse> AddSectionAsync(string name)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var parameters = new { Name = name };

                await connection.ExecuteAsync("AddSection", parameters, commandType: CommandType.StoredProcedure);

                return new ApiResponse(ResponseMessages.SuccessCode, "Section added successfully.");
            }
            catch (SqlException ex) when (ex.Number == 50000) // Handles RAISERROR from SQL
            {
                return new ApiResponse(ResponseMessages.ErrorCode, ex.Message);
            }
            catch (Exception ex)
            {
                return new ApiResponse(ResponseMessages.ErrorCode, ex.Message);
            }
        }

        public async Task<ApiResponse> UpdateSectionAsync(int id, string name)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var parameters = new { Id = id, Name = name };

                await connection.ExecuteAsync("UpdateSection", parameters, commandType: CommandType.StoredProcedure);

                return new ApiResponse(ResponseMessages.SuccessCode, "Section updated successfully.");
            }
            catch (SqlException ex) when (ex.Number == 50000)
            {
                return new ApiResponse(ResponseMessages.ErrorCode, ex.Message);
            }
            catch (Exception ex)
            {
                return new ApiResponse(ResponseMessages.ErrorCode, ex.Message);
            }
        }

        public async Task<ApiResponse> DeleteSectionAsync(int id)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var parameters = new { Id = id };

                await connection.ExecuteAsync("DeleteSection", parameters, commandType: CommandType.StoredProcedure);

                return new ApiResponse(ResponseMessages.SuccessCode, "Section deleted successfully.");
            }
            catch (SqlException ex) when (ex.Number == 50000)
            {
                return new ApiResponse(ResponseMessages.ErrorCode, ex.Message);
            }
            catch (Exception ex)
            {
                return new ApiResponse(ResponseMessages.ErrorCode, ex.Message);
            }
        }

        public async Task<IEnumerable<Section>> GetAllSectionsAsync()
        {
            const string spName = "GetAllSections";

            using var connection = new SqlConnection(_connectionString);
            var sections = await connection.QueryAsync<Section>(
                spName,
                commandType: CommandType.StoredProcedure);

            return sections;
        }

        public async Task<ApiResponse> AddSessionAsync(Session session)
        {
            const string spName = "AddSession";

            using var connection = new SqlConnection(_connectionString);
            await connection.ExecuteAsync(spName, new
            {
                session.CourseId,
                session.Title,
                session.Description
            }, commandType: CommandType.StoredProcedure);

            return new ApiResponse(ResponseMessages.SuccessCode, "Session added successfully.");
        }

        public async Task<ApiResponse> UpdateSessionAsync(Session session)
        {
            const string spName = "UpdateSession";

            using var connection = new SqlConnection(_connectionString);
            await connection.ExecuteAsync(spName, new
            {
                session.Id,
                session.Title,
                session.Description
            }, commandType: CommandType.StoredProcedure);

            return new ApiResponse(ResponseMessages.SuccessCode, "Session updated successfully.");
        }

        public async Task<ApiResponse> DeleteSessionAsync(int sessionId)
        {
            const string spName = "DeleteSession";

            using var connection = new SqlConnection(_connectionString);
            await connection.ExecuteAsync(spName, new { Id = sessionId }, commandType: CommandType.StoredProcedure);

            return new ApiResponse(ResponseMessages.SuccessCode, "Session deleted successfully.");
        }

        public async Task<IEnumerable<Session>> GetAllSessionsAsync()
        {
            const string spName = "GetAllSessionsWithVideos";

            using var connection = new SqlConnection(_connectionString);
            using var multi = await connection.QueryMultipleAsync(
                spName,
                commandType: CommandType.StoredProcedure);

            var sessions = (await multi.ReadAsync<Session>()).ToList();
            var videos = await multi.ReadAsync<SessionVideo>();

            foreach (var session in sessions)
            {
                session.Videos = videos.Where(v => v.SessionId == session.Id).ToList();
            }

            return sessions;
        }


        public async Task<ApiResponse> AddSessionVideoAsync(SessionVideo video)
        {
            const string spName = "AddSessionVideo";

            using var connection = new SqlConnection(_connectionString);
            await connection.ExecuteAsync(spName, new
            {
                video.SessionId,
                video.FileName,
                video.FilePath
            }, commandType: CommandType.StoredProcedure);

            return new ApiResponse(ResponseMessages.SuccessCode, "Video added successfully.");
        }

        public async Task<ApiResponse> DeleteSessionVideoAsync(int videoId)
        {
            const string spName = "DeleteSessionVideo";

            using var connection = new SqlConnection(_connectionString);
            await connection.ExecuteAsync(spName, new { Id = videoId }, commandType: CommandType.StoredProcedure);

            return new ApiResponse(ResponseMessages.SuccessCode, "Video deleted successfully.");
        }

        public async Task<IEnumerable<SessionVideo>> GetSessionVideosAsync(int sessionId)
        {
            const string spName = "GetSessionVideos";

            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryAsync<SessionVideo>(spName, new { SessionId = sessionId }, commandType: CommandType.StoredProcedure);
        }


    }
}
