using TeamsMaker.Api.Contracts.Responses.Profile;

namespace TeamsMaker.Api.Contracts.Requests.Circle;

public class UpdateCircleLinksRequest
{
    public ICollection<LinkInfo>? Links { get; init; } // Todo: Replace LinkInfo
}
