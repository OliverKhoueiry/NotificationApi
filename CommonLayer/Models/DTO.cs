using System.Text.Json.Serialization;

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
public class RoleDto
{
    public int Id { get; set; }
    public string RoleName { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<RoleSectionDto> Sections { get; set; }
}

public class RoleSectionDto
{
    public int SectionId { get; set; }  // This will be mapped from the Section ID (aliased as SectionId in SQL)
    public string SectionName { get; set; }
    public bool IsView { get; set; }
    public bool IsAdd { get; set; }
    public bool IsUpdate { get; set; }
    public bool IsDelete { get; set; }
}

public class CourseDetailsDto
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public int Lessons { get; set; }
    public string Level { get; set; } = "";
    public int DurationWeeks { get; set; }
    public bool OnlineClasses { get; set; }
    public int Quizzes { get; set; }
    public int PassPercentage { get; set; }
    public bool Certificate { get; set; }
    public string Language { get; set; } = "";
    public string CourseImage { get; set; } = "";
    public string CourseSummary { get; set; } = "";
    public string AuthorName { get; set; } = "";
    public string AuthorImage { get; set; } = "";
    public int ReviewsCount { get; set; }
    //public string CourseSummary { get; set; }
    public List<string> WhatYouWillLearn { get; set; } = new();
}

