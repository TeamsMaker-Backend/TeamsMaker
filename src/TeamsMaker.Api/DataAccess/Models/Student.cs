using Core.ValueObjects;

namespace TeamsMaker.Api.DataAccess.Models;

public class Student : User
{
    public string CollegeId { get; set; } = null!;
    public float GPA { get; set; }
    public DateOnly? GraduationYear { get; set; }
    public int Level { get; set; }
    public FileData? CV { get; set; }
    public int DepartmentId { get; set; }
    public virtual User User { get; set; } = null!;
    public virtual Department? Department { get; set; }

    public ICollection<Experience> Experiences { get; set; } = [];
    public ICollection<Project> Projects { get; set; } = [];
}
