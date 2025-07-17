namespace CommonLayer.Models
{
    public class SectionWithActionsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<string> Actions { get; set; } = new();
    }
}
