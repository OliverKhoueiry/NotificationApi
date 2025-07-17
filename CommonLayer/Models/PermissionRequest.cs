namespace CommonLayer.Models
{
    public class PermissionRequest
    {
        public int RoleId { get; set; }
        public int SectionId { get; set; }
        public string Action { get; set; }
    }
}
