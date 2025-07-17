using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommonLayer.Common;
using CommonLayer.Models;

namespace DataLayer
{
    public interface IDataHandler
    {
        Task<User> GetUserByEmailAsync(string email);
        Task<int> AddUserAsync(User user);
        Task UpdateRefreshTokenAsync(int userId, string refreshToken, DateTime expiry);
        Task<List<UserPermission>> GetUserPermissionsAsync(int userId);
        Task<User> GetUserByRefreshTokenAsync(string refreshToken);
        Task SaveResetTokenAsync(int userId, string resetToken, DateTime expiry);
        Task<User> GetUserByResetTokenAsync(string resetToken);
        Task UpdatePasswordAsync(int userId, string hashedPassword);
        Task<List<UserPermission>> GetAllSectionsAndActionsAsync();
        Task ClearRefreshTokenAsync(int userId);
        Task ClearResetTokenAsync(int userId);

        //Task<List<WebRole>> GetWebRolesAsync();
        //Task<WebRole?> GetWebRoleByIdAsync(int id);
        //Task<ApiResponse> AddWebRoleAsync(WebRole role);
        //Task<ApiResponse> UpdateWebRoleAsync(int id, WebRole role);
        
        //Task<List<WebRole>> LoadWebRolesDataAsync();


        Task AddWebRoleAsync(string name);
        Task UpdateWebRoleAsync(int id, string name);
        Task DeleteWebRoleAsync(int id);
        Task<List<WebRole>> GetWebRolesAsync();
        Task AssignPermissionAsync(int roleId, int sectionId, string action);
        Task<List<UserPermission>> GetRolePermissionsAsync(int roleId);


        Task AddRoleSectionAsync(RoleSection roleSection);
        Task UpdateRoleSectionAsync(RoleSection roleSection);
        Task DeleteRoleSectionAsync(int roleSectionId);
        Task<List<RoleSection>> GetRoleSectionsAsync(int roleId);
        Task<RoleSection?> GetRoleSectionByNameAsync(string roleName, string sectionName);

        Task AddReviewAsync(Review review);
        Task<IEnumerable<Review>> GetReviewsByCourseAsync(int courseId);


        Task<IEnumerable<CourseCategory>> GetAllCategoriesAsync();
        Task<IEnumerable<Course>> GetCoursesByCategoryAsync(int categoryId);
        Task AddCourseAsync(Course course);
        Task UpdateCourseAsync(Course course);
        Task DeleteCourseAsync(int courseId);
        Task DeleteReviewAsync(int reviewId);
        Task PromoteUserToAdminAsync(int userId);
        Task AddCategoryAsync(CourseCategory category);
        Task<ApiResponse> DeleteCategoryAsync(int categoryId);



    }
}
