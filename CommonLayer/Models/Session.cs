public class Session
{
    public int Id { get; set; }
    public int CourseId { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public List<SessionVideo>? Videos { get; set; }

}
