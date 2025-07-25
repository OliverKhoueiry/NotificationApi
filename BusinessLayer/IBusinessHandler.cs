using CommonLayer.Common;
using CommonLayer.Dtos;
using CommonLayer.Models;
using static System.Collections.Specialized.BitVector32;

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

        Task<ApiResponse> PromoteUserToRoleAsync(int userId, string roleName);



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
        Task<IEnumerable<Review>> GetAllReviewsAsync();
        Task<IEnumerable<Course>> GetAllCoursesAsync();

        Task<ApiResponse> AddSectionAsync(string name);
        Task<ApiResponse> UpdateSectionAsync(int id, string name);
        Task<ApiResponse> DeleteSectionAsync(int id);


        Task<ApiResponse> AddSessionAsync(Session session);
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
        Task<List<CourseImageDto>> GetAllCourseImagesAsync();
        Task<(ApiResponse response, List<RoleDto> roles)> GetAllRolesAsync();
        Task<(ApiResponse response, RoleDto? role)> GetRoleByIdAsync(int id);
        Task<(ApiResponse response, CourseDetailsDto? course)> GetCourseDetailsAsync(int courseId);
        // CourseLearningOutcomes
        Task<ApiResponse> AddCourseLearningOutcomeAsync(CourseLearningOutcomeDto dto);
        Task<ApiResponse> UpdateCourseLearningOutcomeAsync(CourseLearningOutcomeDto dto);
        Task<ApiResponse> DeleteCourseLearningOutcomeAsync(int id);
        Task<List<CourseLearningOutcomeDto>> GetAllCourseLearningOutcomesAsync(int courseId);

        // CourseSummary
        Task<ApiResponse> AddCourseSummaryAsync(CourseSummaryDto dto);
        Task<ApiResponse> UpdateCourseSummaryAsync(CourseSummaryDto dto);
        Task<ApiResponse> DeleteCourseSummaryAsync(int id);
        Task<CourseSummaryDto?> GetCourseSummaryAsync(int courseId);

    }
}
