using TeamsMaker.Api.Core.Enums;
using TeamsMaker.Api.DataAccess.Base;

namespace TeamsMaker.Api.DataAccess.Models;


public class ApprovalRequest : BaseEntity<Guid>
{
    public bool? IsAccepted { get; set; } = null;
    public Guid ProposalId { get; set; }
    public string StaffId { get; set; } = null!;
    public PositionEnum Position { get; set; }
    public ProposalStatusEnum ProposalStatusSnapshot { get; set; }

    public virtual Proposal Proposal { get; set; } = null!;
    public virtual Staff Staff { get; set; } = null!;
}
