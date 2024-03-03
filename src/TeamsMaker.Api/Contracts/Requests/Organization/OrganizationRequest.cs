namespace TeamsMaker.Api.Contracts.Requests;

public class OrganizationRequest
{
    public required string EngName { get; init; }
    public required string LocName { get; init; }
    public required string Address { get; init; }
    public string? Phone { get; init; }
    public string? Description { get; init; }
    public IFormFile? Logo { get; init; }
}
