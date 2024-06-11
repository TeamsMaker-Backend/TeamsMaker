using TeamsMaker.Api.Contracts.Responses.Circle;
using TeamsMaker.Api.Core.Enums;

namespace TeamsMaker.Api.Contracts.Responses.ApprovalRequest;

public class GetStaffApprovalRequestResponse
{
    public Guid Id { get; set; }
    public bool? IsAccepted { get; set; } = null;
    public Guid ProposalId { get; set; }
    public bool IsProposalReseted { get; set; }
    public ProposalStatusEnum ProposalStatus { get; set; }
    public DateTime? CreationDate { get; set; }
    public GetCircleAsRowResponse CircleResponse { get; set; } = null!;
    public ICollection<GetMemberAsRowResponse> Members { get; set; } = [];
    public ApprovalRequestStaffInfo TargetedStaffInfo { get; set; } = null!;
    public ApprovalRequestStaffInfo? PreviousStaffInfo { get; set; }
}
