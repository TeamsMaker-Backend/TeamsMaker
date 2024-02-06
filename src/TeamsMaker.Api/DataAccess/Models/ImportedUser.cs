namespace TeamsMaker.Api.DataAccess.Models;

public class ImportedUser
{
    public Guid Id { get; set; }
    public string SSN { get; set; } = null!;
    public string Department { get; set; } = null!;
    public bool IsStaff { get; set; }
    public StudentInfo? StudentInfo { get; set; }
}

public class StudentInfo
{
    public string? CollegeId { get; set; }
    public string? GPA { get; set; }
    public DateOnly? GraduationYear { get; set; }
}