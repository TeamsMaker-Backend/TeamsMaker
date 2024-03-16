using Core.ValueObjects;

using TeamsMaker.Api.DataAccess.Base;

namespace TeamsMaker.Api.DataAccess.Models;

public class Permission : BaseEntity<int>
{
    public CircleInfoPermissions CircleInfoPermissions { get; set; } = null!;

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