namespace TeamsMaker.Api.Contracts.Requests;

public class AddOrganizationRequest
{
    public required string EngName { get; init; }
    public required string LocName { get; init; }
    public required string Address { get; init; }
    public string? Phone  { get; init; }
    public string? Description  { get; init; }
    public string? Logo  { get; init; }
}
