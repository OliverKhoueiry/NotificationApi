using BusinessLayer;
using CommonLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
    [Authorize(Roles = "Admin,User")]
    public async Task<IActionResult> GetAllCategories()
    {
        var result = await _businessHandler.GetAllCategoriesAsync();
        return Ok(result);
    }

    [HttpGet("category/{id}")]
    [Authorize(Roles = "Admin,User")]
    public async Task<IActionResult> GetCoursesByCategory(int id)
    {
        var result = await _businessHandler.GetCoursesByCategoryAsync(id);
        return Ok(result);
    }

    [HttpPost("AddCourses")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddCourse([FromBody] Course course)
    {
        var result = await _businessHandler.AddCourseAsync(course);
        return Ok(result);
    }

    [HttpPut("UpdateCourses")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateCourse([FromBody] Course course)
    {
        var result = await _businessHandler.UpdateCourseAsync(course);
        return Ok(result);
    }

    [HttpDelete("DeleteCourses/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteCourse(int id)
    {
        var result = await _businessHandler.DeleteCourseAsync(id);
        return Ok(result);
    }


    [HttpPost("AddCategories")]
    public async Task<IActionResult> AddCategories([FromBody] CourseCategory category)
    {
        var result = await _businessHandler.AddCategoryAsync(category);
        return Ok(result);
    }

}
