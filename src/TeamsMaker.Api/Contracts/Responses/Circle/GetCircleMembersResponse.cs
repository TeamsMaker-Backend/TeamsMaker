using TeamsMaker.Core.Enums;

namespace TeamsMaker.Api.Contracts.Responses.Circle;

public class GetCircleMembersResponse
{
    public ICollection<CircleMemberInfo> Members { get; set; } = [];
}

public class CircleMemberInfo
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? Avatar { get; set; } = null!;
    public Guid CircleMemberId { get; set; }
    public string UserId { get; set; } = null!;
    public UserEnum UserType { get; init; }
    public bool IsOwner { get; set; }
    public string? Badge { get; set; }
    public string? Role { get; set; }
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