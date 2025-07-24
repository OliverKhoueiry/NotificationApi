using BusinessLayer;
using CommonLayer.Common;
using CommonLayer.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CommonLayer.Dtos;


[ApiController]
[Route("api/[controller]")]
public class CoursesController : ControllerBase
{
    private readonly IBusinessHandler _businessHandler;

    public CoursesController(IBusinessHandler businessHandler)
    {
        _businessHandler = businessHandler;
    }

    [HttpGet("GetCategoriesWithCount")]
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
    [HttpGet("loadcategories")]
    public async Task<IActionResult> LoadCategories()
    {
        var categories = await _businessHandler.LoadCategoriesAsync();
        return Ok(categories);
    }
    [HttpPut("UpdateCategory")]
    public async Task<IActionResult> UpdateCategory([FromBody] CategoryDto category)
    {
        var response = await _businessHandler.UpdateCategoryAsync(category);
        return StatusCode(response.Code == ResponseMessages.SuccessCode ? 200 : 400, response);
    }

    [HttpGet("GetCourseById/{id}")]
    public async Task<IActionResult> GetCourseById(int id)
    {
        var course = await _businessHandler.GetCourseByIdAsync(id);
        if (course == null)
        {
            return NotFound(new { message = $"Course with ID {id} not found." });
        }
        return Ok(course);
    }
    [HttpGet("courses/{id}/details")]
    public async Task<IActionResult> GetCourseDetails(int id)
    {
        var (response, course) = await _businessHandler.GetCourseDetailsAsync(id);
        return Ok(new
        {
            response,
            data = course
        });
    }


}
