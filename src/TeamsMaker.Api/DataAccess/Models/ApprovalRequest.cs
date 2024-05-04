using TeamsMaker.Api.Core.Enums;
using TeamsMaker.Api.DataAccess.Base;

namespace TeamsMaker.Api.DataAccess.Models;

public class ApprovalRequest : BaseEntity<Guid>
{
    public bool IsAccepted { get; set; } 

    public Guid ProposalId { get; set; }
    public string StaffId { get; set; } = null!;
    public PositionEnum Position { get; set; } //proposal has one and only one request to each position

    public virtual Proposal Proposal { get; set; } = null!;
    public virtual Staff Staff { get; set; } = null!;
}

/*
ar -> head -> isAccepted -> (record)
ar -> supervisor -> isAccepted = true -> supervisor = staffId
ar(final) -> supervisor + propsal -> isAccepted

*/
/*
ar -> head(reject / cancel) & supervisor & co-supervisor

*/

/*

ar -> co-supervisor -> isAccepted = true -> join circle

*/