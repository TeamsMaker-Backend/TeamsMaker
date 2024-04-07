using TeamsMaker.Api.DataAccess.Base;

namespace TeamsMaker.Api.DataAccess.Models;

public class ApprovalRequest : BaseEntity<Guid>
{
    public bool IsAccepted { get; set; } 

    public Guid ProposalId { get; set; }
    public string StaffId { get; set; } = null!;

    public virtual Proposal Proposal { get; set; } = null!;
    public virtual Staff Staff { get; set; } = null!;
} 
