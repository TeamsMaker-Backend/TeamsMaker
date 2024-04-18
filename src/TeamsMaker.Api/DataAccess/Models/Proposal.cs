using Core.ValueObjects;

using TeamsMaker.Api.Core.Enums;
using TeamsMaker.Api.DataAccess.Base;

namespace TeamsMaker.Api.DataAccess.Models;

public class Proposal : TrackedEntity<Guid>
{
    public string Overview { get; set; } = null!;
    public string Objectives { get; set; } = null!;
    public string TeckStack { get; set; } = null!;
    public ProposalStatusEnum Status { get; set; } = ProposalStatusEnum.NoApproval;

    public Guid CircleId { get; set; }
    public virtual Circle Circle { get; set; } = null!;
    public virtual ICollection<ApprovalRequest> ApprovalRequests { get; set; } = [];
}
