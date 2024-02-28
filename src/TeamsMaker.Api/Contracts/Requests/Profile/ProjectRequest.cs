namespace TeamsMaker.Api.Contracts.Requests.Profile;

public class ProjectRequest
{
    public required string Name { get; init; }
    public required string Url { get; init; }
    public string? Description { get; init; }
    public ICollection<string>? Tags { get; init; }
}