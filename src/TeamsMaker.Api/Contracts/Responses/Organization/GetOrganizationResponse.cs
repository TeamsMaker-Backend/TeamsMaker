namespace TeamsMaker.Api.Contracts.Responses;

public class GetOrganizationResponse
{
    public int Id { get; set; }
    public string EngName { get; set; } = null!;
    public string LocName { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string? Phone { get; set; }
    public string? Description { get; set; }
    public string? Logo { get; set; }
}
