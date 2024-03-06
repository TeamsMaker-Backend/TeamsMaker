using TeamsMaker.Core.Enums;

namespace TeamsMaker.Api.Contracts.Requests.Profile;

public class UpdateProjectRequest
{
    public string? Name { get; init; }
    public string? Url { get; init; }
    public string? Description { get; init; }
    public ICollection<string>? Skills { get; init; }
}
