using BusinessLayer;
using CommonLayer.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class CoursesController : ControllerBase
{
    private readonly IBusinessHandler _businessHandler;

    public CoursesController(IBusinessHandler businessHandler)
    {
        _businessHandler = businessHandler;
    }

    [HttpGet("categories")]
    public async Task<IActionResult> GetAllCategories()
    {
        var result = await _businessHandler.GetAllCategoriesAsync();
        return Ok(result);
    }

    [HttpGet("category/{id}")]
    public async Task<IActionResult> GetCoursesByCategory(int id)
    {
        var result = await _businessHandler.GetCoursesByCategoryAsync(id);
        return Ok(result);
    }

    [HttpPost("AddCourses")]
    public async Task<IActionResult> AddCourse([FromBody] Course course)
    {
        var result = await _businessHandler.AddCourseAsync(course);
        return Ok(result);
    }

    [HttpPut("UpdateCourses")]
    public async Task<IActionResult> UpdateCourse([FromBody] Course course)
    {
        var result = await _businessHandler.UpdateCourseAsync(course);
        return Ok(result);
    }

    [HttpDelete("DeleteCourses/{id}")]
    public async Task<IActionResult> DeleteCourse(int id)
    {
        var result = await _businessHandler.DeleteCourseAsync(id);
        return Ok(result);
    }
    [HttpGet("AllCourses")]
    public async Task<IActionResult> GetAllCourses()
    {
        var courses = await _businessHandler.GetAllCoursesAsync();
        return Ok(courses);
    }

    [HttpPost("AddCategories")]
    public async Task<IActionResult> AddCategories([FromBody] CourseCategory category)
    {
        var result = await _businessHandler.AddCategoryAsync(category);
        return Ok(result);
    }

    [HttpDelete("DeleteCategory/{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var result = await _businessHandler.DeleteCategoryAsync(id);
        return Ok(result);
    }
}
