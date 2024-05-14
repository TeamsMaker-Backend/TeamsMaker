using TeamsMaker.Api.Core.Enums;
using TeamsMaker.Api.DataAccess.Base;

namespace TeamsMaker.Api.DataAccess.Models;

//TODO: add contact
public class Proposal : TrackedEntity<Guid>
{
    public string Overview { get; set; } = null!;
    public string Objectives { get; set; } = null!;
    public string TechStack { get; set; } = null!;
    public bool IsReseted { get; set; }
    public ProposalStatusEnum Status { get; set; } = ProposalStatusEnum.NoApproval;

    public Guid CircleId { get; set; }
    public virtual Circle Circle { get; set; } = null!;
    public virtual ICollection<ApprovalRequest> ApprovalRequests { get; set; } = [];
}
