using TeamsMaker.Api.DataAccess.Base;

namespace TeamsMaker.Api.DataAccess.Models;

public class Permission : BaseEntity<int>
{
    public string Name { get; set; } = null!;

    public virtual ICollection<MemberPermission> MemberPermissions { get; set; } = [];
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
feed general
activity log: circle, scroll down: profile: go to posts

*/