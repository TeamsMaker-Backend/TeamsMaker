using TeamsMaker.Api.Core.Enums;

namespace TeamsMaker.Api.Contracts.Responses.ApprovalRequest;

public class GetStaffApprovalRequestResponse
{
    public Guid Id { get; set; }
    public string? CircleAvatar { get; set; }
    public bool? IsAccepted { get; set; } = null;
    public Guid ProposalId { get; set; }
    public ProposalStatusEnum ProposalStatus { get; set; }
    public ApprovalRequestStaffInfo TargetedStaffInfo { get; set; } = null!;
    public ApprovalRequestStaffInfo? PreviousStaffInfo { get; set; }
}
