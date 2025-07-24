using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommonLayer.Common;
using CommonLayer.Dtos;
using CommonLayer.Models;
using static System.Collections.Specialized.BitVector32;


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
        Task PromoteUserToRoleAsync(int userId, string roleName);

        Task AddCategoryAsync(CourseCategory category);
        Task<ApiResponse> DeleteCategoryAsync(int categoryId);

        Task<IEnumerable<Review>> GetAllReviewsAsync();
        Task<IEnumerable<Course>> GetAllCoursesAsync();

        Task<ApiResponse> AddSectionAsync(string name);
        Task<ApiResponse> UpdateSectionAsync(int id, string name);
        Task<ApiResponse> DeleteSectionAsync(int id);
        Task<IEnumerable<Section>> GetAllSectionsAsync();

        Task AddSessionAsync(Session session);
        Task<ApiResponse> UpdateSessionAsync(Session session);
        Task<ApiResponse> DeleteSessionAsync(int sessionId);
        Task<IEnumerable<Session>> GetAllSessionsAsync();

        Task<ApiResponse> AddSessionVideoAsync(SessionVideo video);
        Task<ApiResponse> DeleteSessionVideoAsync(int videoId);
        Task<IEnumerable<SessionVideo>> GetSessionVideosAsync(int sessionId);
        Task<List<CategoryDto>> LoadCategoriesAsync();
        Task<ApiResponse> UpdateCategoryAsync(CategoryDto category);
        Task<Course?> GetCourseByIdAsync(int courseId);
        Task<List<UserDto>> GetAllUsersAsync();

        Task<ApiResponse> AddUserAsync(UserDto userDto);
        Task<ApiResponse> UpdateUserAsync(int userId, UpdateUserDto userDto);
        Task<ApiResponse> DeleteUserAsync(int userId);
        Task<ApiResponse> AddRoleAsync(AddRoleDto roleDto);
        Task<ApiResponse> UpdateRoleAsync(int roleId, AddRoleDto roleDto);

        Task<HomeResponseDto> GetHomeDataAsync();

        Task<List<CourseImageDto>> GetAllCourseImagesAsync();
        Task<ApiResponse> AddCourseImageAsync(CourseImageDto dto);
        Task<ApiResponse> UpdateCourseImageAsync(CourseImageDto dto);
        Task<ApiResponse> DeleteCourseImageAsync(int id);
        Task<List<CategoryImageDto>> GetAllCategoryImagesAsync();
        Task<ApiResponse> AddCategoryImageAsync(CategoryImageDto dto);
        Task<ApiResponse> UpdateCategoryImageAsync(CategoryImageDto dto);
        Task<ApiResponse> DeleteCategoryImageAsync(int id);

        Task<List<AuthorDto>> GetAllAuthorsAsync();
        Task<ApiResponse> AddAuthorAsync(AuthorDto dto);
        Task<ApiResponse> UpdateAuthorAsync(AuthorDto dto);
        Task<ApiResponse> DeleteAuthorAsync(int id);
        Task<(ApiResponse response, List<RoleDto> roles)> GetAllRolesAsync();
        Task<(ApiResponse response, RoleDto? role)> GetRoleByIdAsync(int id);

    }
}
