using TeamsMaker.Api.Core.Enums;

namespace TeamsMaker.Api.Contracts.Responses.ApprovalRequest;

public class ListStaffApprovalRequestResponse
{
    public ICollection<StaffApprovalRequestInfo> ApprovalRequests { get; set; } = [];
}

public class StaffApprovalRequestInfo
{
    public Guid Id { get; set; }
    public bool? IsAccepted { get; set; } = null;
    public Guid ProposalId { get; set; }
    public ProposalStatusEnum ProposalStatus { get; set; }
    public StaffInfo TargetedStaffInfo { get; set; } = null!;
    public StaffInfo? PreviousStaffInfo { get; set; }
}
