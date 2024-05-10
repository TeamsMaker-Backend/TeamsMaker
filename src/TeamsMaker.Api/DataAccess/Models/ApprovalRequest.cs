using TeamsMaker.Api.Core.Enums;
using TeamsMaker.Api.DataAccess.Base;

namespace TeamsMaker.Api.DataAccess.Models;

 
public class ApprovalRequest : BaseEntity<Guid>
{
    public bool IsAccepted { get; set; } //TODO: nullable Reset: case
    public Guid ProposalId { get; set; }
    public string StaffId { get; set; } = null!;
    public PositionEnum Position { get; set; } //proposal has one and only one request to each position

    public virtual Proposal Proposal { get; set; } = null!;
    public virtual Staff Staff { get; set; } = null!;
}

/*
ar -> head(reject / cancel) & supervisor & co-supervisor

*/

/*

ar -> co-supervisor -> isAccepted = true -> join circle

*/

/*
record sent to head : accepted  Supervisor =  null


record sent to supervisor: accepted  Supervisor =  value

Get staff:
trick: 1st, (2nd: include head name)  (3rd: include supervisor name)


-------------------------
NoApproval, no ApprovalRequest isAccepted , Position: head


Post: Send
Delete:  Cancel: delete
Patch:  Deny: false
Patch: Accept: true
Get student
Get staff,  get id
*/