using TeamsMaker.Api.Core.Enums;

namespace TeamsMaker.Api.Contracts.Responses.ApprovalRequest;

public class ListCircleApprovalRequestResponse
{
    public ICollection<CircleApprovalRequestInfo> ApprovalRequests { get; set; } = [];
}

public class CircleApprovalRequestInfo
{
    public Guid Id { get; set; }
    public bool? IsAccepted { get; set; } = null;
    public Guid ProposalId { get; set; }
    public ProposalStatusEnum ProposalStatus { get; set; }
    public StaffInfo TargetedStaffInfo { get; set; } = null!;
}

public class StaffInfo
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public PositionEnum Position { get; set; }
}
