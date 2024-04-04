namespace TeamsMaker.Api.Contracts.Responses.Circle;

public class GetCircleMembersResponse
{
    public ICollection<CircleMemberInfo> Members { get; set; } = [];
}

public class CircleMemberInfo
{
    public Guid CircleMemberId { get; set; }
    public string UserId { get; set; } = null!;
    public bool IsOwner { get; set; }
    public string? Badge { get; set; }
    public string? Bio { get; set; }
    public PermissionsInfo? ExceptionPermissions { get; set; } = null!;
}

public class PermissionsInfo
{
    public bool MemberManagement { get; set; }
    public bool CircleManagment { get; set; }
    public bool ProposalManagment { get; set; }
    public bool FeedManagment { get; set; }
    public bool SessionManagment { get; set; }
    public bool TodoTaskManagment { get; set; }
}