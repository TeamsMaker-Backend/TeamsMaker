using TeamsMaker.Api.Contracts.Responses.Profile;

namespace TeamsMaker.Api.Contracts.Requests.Circle;

public class UpdateCircleInfoRequest
{
    public required string Name { get; init; }
    public ICollection<string>? Keywords { get; init; }
    public string? Summary { get; init; }
    public ICollection<string>? Skills { get; init; }
    public ICollection<LinkInfo>? Links { get; init; }
}
