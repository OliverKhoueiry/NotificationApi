public class AddRoleDto
{
    public string RoleName { get; set; }
    public List<SectionDto> Sections { get; set; }
}

public class SectionDto
{
    public int Id { get; set; }
    public List<bool> Actions { get; set; } // True/False for IsView, IsUpdate, IsDelete
}
