using BusinessLayer;
using CommonLayer.Common;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class SessionController : ControllerBase
{
    private readonly IWebHostEnvironment _env;
    private readonly IBusinessHandler _businessHandler;

    public SessionController(IBusinessHandler businessHandler, IWebHostEnvironment env)
    {
        _businessHandler = businessHandler;
        _env = env;
    }

    [HttpGet("GetSessionAndVideos")]
    public async Task<IActionResult> GetAllSessions()
    {
        var sessions = await _businessHandler.GetAllSessionsAsync();
        return Ok(sessions);
    }

    [HttpPost("AddSession")]
    public async Task<IActionResult> AddSession([FromBody] Session session)
    {
        var response = await _businessHandler.AddSessionAsync(session);
        return StatusCode(response.Code == ResponseMessages.SuccessCode ? 200 : 400, response);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSession(int id, [FromBody] Session session)
    {
        session.Id = id;
        var response = await _businessHandler.UpdateSessionAsync(session);
        return StatusCode(response.Code == ResponseMessages.SuccessCode ? 200 : 400, response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSession(int id)
    {
        var response = await _businessHandler.DeleteSessionAsync(id);
        return StatusCode(response.Code == ResponseMessages.SuccessCode ? 200 : 400, response);
    }

    [HttpPost("{sessionId}/videos")]
    public async Task<IActionResult> UploadVideo(int sessionId, IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded.");

        // Create a directory path
        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
        if (!Directory.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(uploadsFolder);
        }

        // Combine full file path
        var fileName = $"{Guid.NewGuid()}_{file.FileName}";
        var filePath = Path.Combine(uploadsFolder, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        // Save to DB
        var video = new SessionVideo
        {
            SessionId = sessionId,
            Title = Path.GetFileNameWithoutExtension(file.FileName),
            FileName = fileName,
            FilePath = $"/uploads/{fileName}",
            UploadDate = DateTime.UtcNow
        };

        await _businessHandler.AddSessionVideoAsync(video);

        return Ok(new { message = "File uploaded successfully.", video });
    }


    [HttpGet("{sessionId}/videos")]
    public async Task<IActionResult> GetSessionVideos(int sessionId)
    {
        var videos = await _businessHandler.GetSessionVideosAsync(sessionId);
        return Ok(videos);
    }

    [HttpDelete("videos/{videoId}")]
    public async Task<IActionResult> DeleteVideo(int videoId)
    {
        var response = await _businessHandler.DeleteSessionVideoAsync(videoId);
        return StatusCode(response.Code == ResponseMessages.SuccessCode ? 200 : 400, response);
    }
}
