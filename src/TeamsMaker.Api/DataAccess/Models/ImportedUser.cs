namespace TeamsMaker.Api.DataAccess.Models;

public class ImportedUser
{
    public Guid Id { get; set; }
    public string SSN { get; set; } = null!;
    public bool IsStaff { get; set; }
    public int OrganizationId { get; set; }

    public string? CollegeId { get; set; }
    public float? GPA { get; set; }
    public DateOnly? GraduationYear { get; set; }
    public string? Department { get; set; }
}