using TeamsMaker.Api.DataAccess.Base;

namespace TeamsMaker.Api.DataAccess.Models;

public class Permission : BaseEntity<int>
{
    public bool MemberManagement { get; set; } = false;
    public bool CircleManagment { get; set; } = false;
    public bool ProposalManagment { get; set; } = false;
    public bool FeedManagment { get; set; } = false;

    public Guid CircleId { get; set; }
    public Circle Circle { get; set; } = null!;
    public Guid CircleMemberId { get; set; }
    public virtual CircleMember CircleMember { get; set; } = null!;
}

/*
owner: 
    edit , name, logo, header, descripion, 
    add: invite member, accept member request , remove member,

    posts,
    send proposal 
    delete: circle, 
*/

/*
 - Danger Zone:
    TransferOwnerShip
    ArchiveCircle
    DeleteCiCle
    
- MemberManagement
    
- CircleManagment

- ProposalMangment

- FeedManagment
 */