using TeamsMaker.Api.Core.Enums;

namespace TeamsMaker.Api.Contracts.Responses.ApprovalRequest;

public class GetCircleApprovalRequestResponse
{
    public Guid Id { get; set; }
    public bool? IsAccepted { get; set; } = null;
    public Guid ProposalId { get; set; }
    public ProposalStatusEnum ProposalStatus { get; set; }
    public ApprovalRequestStaffInfo TargetedStaffInfo { get; set; } = null!;
}

public class ApprovalRequestStaffInfo
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public PositionEnum Position { get; set; }
    public string? Avatar { get; set; }
}
