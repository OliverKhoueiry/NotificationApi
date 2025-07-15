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
        Task<int> UpdatePasswordAsync(int userId, string newHashedPassword);
        Task ClearRefreshTokenAsync(int userId);




        Task<IEnumerable<CourseCategory>> GetAllCategoriesAsync();
        Task<IEnumerable<Course>> GetCoursesByCategoryAsync(int categoryId);
        Task<int> AddCourseAsync(Course course);
        Task<int> UpdateCourseAsync(Course course);
        Task<int> DeleteCourseAsync(int courseId);



        Task<int> AddReviewAsync(Review review);
        Task<IEnumerable<Review>> GetReviewsByCourseAsync(int courseId);
        Task<int> DeleteReviewAsync(int reviewId);

        Task<int> UpdateUserRoleAsync(int userId, string role);

        Task<int> AddCategoryAsync(CourseCategory category);

        Task<int> DeleteCategoryAsync(int categoryId);

    }
}
