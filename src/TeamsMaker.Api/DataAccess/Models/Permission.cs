using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;

using TeamsMaker.Api.DataAccess.Base;

namespace TeamsMaker.Api.DataAccess.Models;

public class Permission : BaseEntity<int>
{
    public string Name { get; set; } = null!;
    public string? Group { get; set; } // PermissionGroups const
    public string? Description { get; set; }

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