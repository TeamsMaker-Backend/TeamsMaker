namespace TeamsMaker.Api.DataAccess.Models;

public class ImportedStaff
{
    public Guid Id { get; set; }
    public string SSN { get; set; } = null!;
    public int OrganizationId { get; set; }
    //TODO: add role or classification
}
