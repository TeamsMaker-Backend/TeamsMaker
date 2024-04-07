namespace TeamsMaker.Api.DataAccess.Models;

public class ImportedStaff
{
    public Guid Id { get; set; }
    public string SSN { get; set; } = null!;
    public StaffClassificationsEnum? Classification { get; set; }
    public int OrganizationId { get; set; }
}