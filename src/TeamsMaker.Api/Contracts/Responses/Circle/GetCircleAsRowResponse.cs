using TeamsMaker.Api.Contracts.Responses.JoinRequest;
using TeamsMaker.Api.Contracts.Responses.Profile;
using TeamsMaker.Core.Enums;

namespace TeamsMaker.Api.Contracts.Responses.Circle;

public class GetCircleAsRowResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Avatar { get; set; }
    public string OwnerName { get; set; } = null!;
}