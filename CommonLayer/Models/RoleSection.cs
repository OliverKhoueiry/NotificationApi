namespace CommonLayer.Models
{
    public class RoleSection
    {
        public int Id { get; set; }
        public int IdWebRole { get; set; }
        public int IdSection { get; set; }
        public bool IsView { get; set; }
        public bool IsAdd { get; set; }
        public bool IsUpdate { get; set; }
        public bool IsDelete { get; set; }
        public string SectionName { get; set; }
    }
}
