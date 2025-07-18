public class SessionVideo
{
    public int Id { get; set; }
    public int SessionId { get; set; }
    public string FileName { get; set; } = null!;
    public string FilePath { get; set; } = null!;
    public DateTime UploadDate { get; set; }
    public string Title { get; set; }
}
