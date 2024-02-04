namespace TeamsMaker.Api.Contracts.Responses;

public class GetOrganizationResponse
{
    public int Id { get; init; }
    public required string EngName { get; init; }
    public required string LocName { get; init; }
    public required string Address { get; init; }
    public string? Phone { get; init; }
    public string? Description { get; init; }
    public byte[]? Logo { get; init; }
}
