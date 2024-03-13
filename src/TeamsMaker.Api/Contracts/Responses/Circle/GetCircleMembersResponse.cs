using Core.ValueObjects;

namespace TeamsMaker.Api.Contracts.Responses.Circle;

public class GetCircleMembersResponse
{
    public ICollection<CircleMemberInfo> Members { get; set; } = [];
}

public class CircleMemberInfo
{
    public string UserId { get; set; } = null!;
    public bool IsOwner { get; set; }
    public string? Badge { get; set; }
    public CircleInfoPermissions Permissions { get; set; } = null!;
}