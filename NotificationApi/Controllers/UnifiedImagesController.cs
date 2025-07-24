using Microsoft.AspNetCore.Mvc;
using BusinessLayer;
using CommonLayer.Common;
using CommonLayer.Dtos;
//using BusinessLayer;

namespace NotificationApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImagesController : ControllerBase
    {
        private readonly IBusinessHandler _businessHandler;

        public ImagesController(IBusinessHandler businessHandler)
        {
            _businessHandler = businessHandler;
        }

        // ===== CourseImages =====

        [HttpGet("course-images")]
        public async Task<ActionResult<List<CourseImageDto>>> GetAllCourseImages()
        {
            var result = await _businessHandler.GetAllCourseImagesAsync();
            return Ok(result);
        }

        [HttpPost("course-images")]
        public async Task<ActionResult<ApiResponse>> AddCourseImage([FromBody] CourseImageDto dto)
        {
            var response = await _businessHandler.AddCourseImageAsync(dto);
            return Ok(response);
        }

        [HttpPut("course-images")]
        public async Task<ActionResult<ApiResponse>> UpdateCourseImage([FromBody] CourseImageDto dto)
        {
            var response = await _businessHandler.UpdateCourseImageAsync(dto);
            return Ok(response);
        }

        [HttpDelete("course-images/{id}")]
        public async Task<ActionResult<ApiResponse>> DeleteCourseImage(int id)
        {
            var response = await _businessHandler.DeleteCourseImageAsync(id);
            return Ok(response);
        }

        // ===== CategoryImages =====

        [HttpGet("category-images")]
        public async Task<ActionResult<List<CategoryImageDto>>> GetAllCategoryImages()
        {
            var result = await _businessHandler.GetAllCategoryImagesAsync();
            return Ok(result);
        }

        [HttpPost("category-images")]
        public async Task<ActionResult<ApiResponse>> AddCategoryImage([FromBody] CategoryImageDto dto)
        {
            var response = await _businessHandler.AddCategoryImageAsync(dto);
            return Ok(response);
        }

        [HttpPut("category-images")]
        public async Task<ActionResult<ApiResponse>> UpdateCategoryImage([FromBody] CategoryImageDto dto)
        {
            var response = await _businessHandler.UpdateCategoryImageAsync(dto);
            return Ok(response);
        }

        [HttpDelete("category-images/{id}")]
        public async Task<ActionResult<ApiResponse>> DeleteCategoryImage(int id)
        {
            var response = await _businessHandler.DeleteCategoryImageAsync(id);
            return Ok(response);
        }

        // ===== Author =====

        [HttpGet("authors")]
        public async Task<ActionResult<List<AuthorDto>>> GetAllAuthors()
        {
            var result = await _businessHandler.GetAllAuthorsAsync();
            return Ok(result);
        }

        [HttpPost("authors")]
        public async Task<ActionResult<ApiResponse>> AddAuthor([FromBody] AuthorDto dto)
        {
            var response = await _businessHandler.AddAuthorAsync(dto);
            return Ok(response);
        }

        [HttpPut("authors")]
        public async Task<ActionResult<ApiResponse>> UpdateAuthor([FromBody] AuthorDto dto)
        {
            var response = await _businessHandler.UpdateAuthorAsync(dto);
            return Ok(response);
        }

        [HttpDelete("authors/{id}")]
        public async Task<ActionResult<ApiResponse>> DeleteAuthor(int id)
        {
            var response = await _businessHandler.DeleteAuthorAsync(id);
            return Ok(response);
        }
    }
}
