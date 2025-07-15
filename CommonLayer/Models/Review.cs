public class Review
{
    public int Id { get; set; }
    public int CourseId { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string ReviewComment { get; set; } = null!;
    public int StarsOfTheReview { get; set; }
    public DateTime ReviewDate { get; set; }
}
