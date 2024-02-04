namespace TeamsMaker.Api.Contracts.Requests;

public class UpdateOrganizationRequest
{
    public string? EngName { get; init; }
    public string? LocName { get; init; }
    public string? Address { get; init; }
    public string? Phone  { get; init; }
    public string? Description  { get; init; }
    public byte[]? Logo  { get; init; }
}
