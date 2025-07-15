using BusinessLayer;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ReviewsController : ControllerBase
{
    private readonly IBusinessHandler _businessHandler;

    public ReviewsController(IBusinessHandler businessHandler)
    {
        _businessHandler = businessHandler;
    }

    [HttpPost]
    public async Task<IActionResult> AddReview([FromBody] Review review)
    {
        var response = await _businessHandler.AddReviewAsync(review);
        return Ok(response);
    }

    [HttpGet("course/{courseId}")]
    public async Task<IActionResult> GetReviewsByCourse(int courseId)
    {
        var reviews = await _businessHandler.GetReviewsByCourseAsync(courseId);
        return Ok(reviews);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteReview(int id)
    {
        var response = await _businessHandler.DeleteReviewAsync(id);
        return Ok(response);
    }
}
