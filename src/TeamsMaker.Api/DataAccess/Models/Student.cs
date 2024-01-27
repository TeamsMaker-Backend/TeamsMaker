namespace TeamsMaker.Api.DataAccess.Models;

public class Student : User
{
    public string CollegeId { get; set; } = string.Empty;
    public float GPA { get; set; }
    public DateOnly GraduationYear { get; set; }
    public int Level { get; set; }
    public byte[]? CV { get; set; }
}
