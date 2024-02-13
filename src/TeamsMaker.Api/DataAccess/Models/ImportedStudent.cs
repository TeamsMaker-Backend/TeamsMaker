namespace TeamsMaker.Api.DataAccess.Models;

public class ImportedStudent
{
    public Guid Id { get; set; }
    public string SSN { get; set; } = null!;
    public string CollegeId { get; set; } = null!;
    public float GPA { get; set; }
    public DateOnly GraduationYear { get; set; }
    public string Department { get; set; } = null!;
    public int OrganizationId { get; set; }
}