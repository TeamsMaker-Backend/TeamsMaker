using Core.ValueObjects;

using TeamsMaker.Api.Contracts.Responses.Circle;
using TeamsMaker.Api.Contracts.Responses.Profile;

namespace TeamsMaker.Api.Contracts.Requests.Circle;

public class AddCircleRequest
{
    public required string Name { get; init; }
    public string? Description { get; init; }
    public SummaryData? Summary { get; init; }
    public ICollection<LinkInfo>? Links { get; init; }
    public ICollection<string>? Skills { get; init; }
    public required PermissionsInfo DefaultPermission { get; init; }
}
