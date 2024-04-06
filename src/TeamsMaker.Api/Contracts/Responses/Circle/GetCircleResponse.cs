using TeamsMaker.Api.Contracts.Responses.JoinRequest;
using TeamsMaker.Api.Contracts.Responses.Profile;
using TeamsMaker.Core.Enums;

namespace TeamsMaker.Api.Contracts.Responses.Circle;

public class GetCircleResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public ICollection<string> Keywords { get; set; } = [];
    public string? Summary { get; set; }
    public bool IsPublic { get; set; }
    public string? Avatar { get; set; }
    public string? Header { get; set; }
    public long Rate { get; set; }
    public CircleStatusEnum Status { get; set; }
    public int OrganizationId { get; set; }
    public PermissionsInfo DefaultPermission { get; set; } = null!;
    public GetJoinRequestResponse? CircleJoinRequests { get; set; }
    public ICollection<LinkInfo>? Links { get; set; } = []; // Replace LinkInfo
    public ICollection<string>? Skills { get; set; } = [];
}
