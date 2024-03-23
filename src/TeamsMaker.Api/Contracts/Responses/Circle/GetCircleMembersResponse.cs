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
    public PermissionsInfo? Permissions { get; set; } = null!;
}

public class PermissionsInfo
{
    public bool MemberManagement { get; set; }
    public bool CircleManagment { get; set; }
    public bool ProposalManagment { get; set; }
    public bool FeedManagment { get; set; }
}