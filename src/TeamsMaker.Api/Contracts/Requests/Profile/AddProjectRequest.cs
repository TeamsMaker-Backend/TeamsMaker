namespace TeamsMaker.Api.Contracts.Requests.Profile;

public class AddProjectRequest
{
    public required string Name { get; init; }
    public string? Url { get; init; }
    public string? Description { get; init; }
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public ICollection<string>? Skills { get; init; }
}