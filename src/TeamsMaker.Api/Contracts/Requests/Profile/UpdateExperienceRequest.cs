namespace TeamsMaker.Api.Contracts.Requests.Profile;

public class UpdateExperienceRequest
{
    public string? Title { get; init; }
    public string? Organization { get; init; }
    public string? Role { get; init; }
    public DateOnly? StartDate { get; init; }
    public DateOnly? EndDate { get; init; }
    public string? Description { get; init; }
}