namespace CommonLayer.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Overview { get; set; }
        public decimal Price { get; set; }
        public string Level { get; set; }
        public int DurationWeeks { get; set; }
        public int OnlineClasses { get; set; }
        public int Lessons { get; set; }
        public int Quizzes { get; set; }
        public int PassPercentage { get; set; }
        public bool Certificate { get; set; }
        public string Language { get; set; }
        public int CategoryId { get; set; }
       
    }
}
