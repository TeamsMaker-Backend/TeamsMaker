namespace TeamsMaker.Api.DataAccess.Models;

public class Student : User
{
    public string CollegeId { get; set; } = null!;
    public float GPA { get; set; }
    public DateOnly? GraduationYear { get; set; }
    public int Level { get; set; }
    public string? CV { get; set; }

    public static Student Create(string firstName, string lastName, string email, string userName)
    {
        return new Student
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            UserName = userName
        };
    }

    // public Student WithCollegeId()
}
