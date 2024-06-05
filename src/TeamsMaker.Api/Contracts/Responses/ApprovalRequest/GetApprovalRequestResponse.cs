using TeamsMaker.Api.Contracts.Responses.Circle;
using TeamsMaker.Api.Contracts.Responses.Proposal;
using TeamsMaker.Api.Core.Enums;

namespace TeamsMaker.Api.Contracts.Responses.ApprovalRequest;

public class GetApprovalRequestResponse
{
    public DateTime? CreationDate { get; set; }
    public bool? IsAccepted { get; set; } = null;
    public Guid ProposalId { get; set; }
    public bool IsProposalReseted { get; set; }
    public PositionEnum Position { get; set; }
    public ProposalStatusEnum ProposalStatus { get; set; }
    public ApprovalRequestStaffInfo TargetedStaffInfo { get; set; } = null!;
    public GetProposalResponse ProposalResponse { get; set; } = null!;
    public GetCircleAsRowResponse CircleResponse { get; set; } = null!;
    public ICollection<GetMemberAsRowResponse> Members { get; set; } = [];
}

public class GetMemberAsRowResponse
{
    public string UserId { get; set; } = null!;
    public bool IsOwner { get; set; }
    public string? Avatar {  get; set; }
    public string Name { get; set; } = null!;
    public string? Badge { get; set; }
}
