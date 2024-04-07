using Core.ValueObjects;

using TeamsMaker.Api.Core.Enums;
using TeamsMaker.Api.DataAccess.Base;

namespace TeamsMaker.Api.DataAccess.Models;

public class Proposal : TrackedEntity<Guid>
{
    public FileData? File { get; set; } // proposal
    public ProposalStatusEnum Status { get; set; } = ProposalStatusEnum.NoApproval;

    public Guid CircleId { get; set; }
    public virtual Circle Circle { get; set; } = null!;
    public virtual ICollection<ApprovalRequest> ApprovalRequests { get; set; } = [];
}
