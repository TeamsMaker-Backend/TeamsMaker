namespace TeamsMaker.Api.Contracts.Requests.Profile;

public class AddExperienceRequest
{
    public required string Organization { get; init; }
    public required string Role { get; init; }
    public DateOnly? StartDate { get; init; }
    public DateOnly? EndDate { get; init; }
    public string? Description { get; init; }
}