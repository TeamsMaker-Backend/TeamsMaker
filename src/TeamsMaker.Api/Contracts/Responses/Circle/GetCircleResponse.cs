using Core.ValueObjects;

using TeamsMaker.Api.Contracts.Responses.Profile;
using TeamsMaker.Core.Enums;

namespace TeamsMaker.Api.Contracts.Responses.Circle;

public class GetCircleResponse
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? Summary { get; set; }
    public bool IsPublic { get; set; }
    public string? Avatar { get; set; }
    public string? Header { get; set; }
    public long Rate { get; set; }
    public CircleStatusEnum Status { get; set; }
    public int OrganizationId { get; set; }
    public ICollection<LinkInfo>? Links { get; set; } = []; // Replace LinkInfo
    public ICollection<string>? Skills { get; set; } = [];
    public ICollection<CircleMemberInfo> Members { get; set; } = [];
}

public class CircleMemberInfo
{
    public string UserId { get; set; } = null!;
    public bool IsOwner { get; set; }
    public string? Badge { get; set; }
    public CircleInfoPermissions Permissions { get; set; } = null!;
}
