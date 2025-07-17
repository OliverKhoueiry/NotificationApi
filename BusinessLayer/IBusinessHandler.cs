using CommonLayer.Common;
using CommonLayer.Models;

namespace BusinessLayer
{
    public interface IBusinessHandler
    {
        Task<ApiResponse> RegisterAsync(RegisterRequest request);
        Task<(ApiResponse, string?, string?, List<UserPermission>)> LoginAsync(LoginRequest request);


        Task<ApiResponse> ForgetPasswordAsync(string email);
        Task<(ApiResponse, string?, string?)> RefreshTokenAsync(string refreshToken);
        Task<ApiResponse> ResetPasswordAsync(ResetPasswordRequest request);
        Task<ApiResponse> LogoutAsync(string refreshToken);



        Task<IEnumerable<CourseCategory>> GetAllCategoriesAsync();
        Task<IEnumerable<Course>> GetCoursesByCategoryAsync(int categoryId);
        Task<ApiResponse> AddCourseAsync(Course course);
        Task<ApiResponse> UpdateCourseAsync(Course course);
        Task<ApiResponse> DeleteCourseAsync(int courseId);


        Task<ApiResponse> AddReviewAsync(Review review);
        Task<IEnumerable<Review>> GetReviewsByCourseAsync(int courseId);
        Task<ApiResponse> DeleteReviewAsync(int reviewId);

        Task<ApiResponse> PromoteUserToAdminAsync(int userId);


        Task<ApiResponse> AddCategoryAsync(CourseCategory category);

        Task<ApiResponse> DeleteCategoryAsync(int categoryId);
        
        
         
        Task<List<UserPermission>> GetAllSectionsAsync();


        Task<ApiResponse> AddWebRoleAsync(string name);
        Task<ApiResponse> UpdateWebRoleAsync(int id, string name);
        Task<ApiResponse> DeleteWebRoleAsync(int id);
        Task<List<WebRole>> GetWebRolesAsync();
        Task<ApiResponse> AssignPermissionAsync(int roleId, int sectionId, string action);
        Task<List<UserPermission>> GetRolePermissionsAsync(int roleId);


        Task<ApiResponse> AddRoleSectionAsync(RoleSection roleSection);
        Task<ApiResponse> UpdateRoleSectionAsync(RoleSection roleSection);
        Task<ApiResponse> DeleteRoleSectionAsync(int roleSectionId);
        Task<List<RoleSection>> GetRoleSectionsAsync(int roleId);
        Task<RoleSection?> GetRoleSectionByNameAsync(string roleName, string sectionName);
    }
}
