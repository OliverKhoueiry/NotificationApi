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
public class HomeCategoryItemDto
{
    public int Id { get; set; }
    public string CategoryImagePath { get; set; }
    public string Name { get; set; }
    public int CourseCount { get; set; }
}

public class HomeCourseItemDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string CourseImagePath { get; set; }
    public int Lessons { get; set; }
    public int ReviewCount { get; set; }
    public string Description { get; set; }
    public string AuthorName { get; set; }
    public string AuthorImage { get; set; }
}

public class HomeResponseDto
{
    public List<HomeCategoryItemDto> Categories { get; set; }
    public List<HomeCourseItemDto> Courses { get; set; }
}

public class CourseImageDto
{
    public int Id { get; set; }
    public int CourseId { get; set; }
    public string ImagePath { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CategoryImageDto
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public string ImagePath { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class AuthorDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string PhotoPath { get; set; }
    public int IdCourse { get; set; }
}
