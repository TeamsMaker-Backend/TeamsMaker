using Core.ValueObjects;

using TeamsMaker.Api.DataAccess.Base;
using TeamsMaker.Core.Enums;

namespace TeamsMaker.Api.DataAccess.Models;

public class Circle : TrackedEntity<Guid>, IReadOnlyOrganizationInfo
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public FileData? Avatar { get; set; }
    public FileData? Header { get; set; } // back ground
    public CircleStatusEnum Status { get; set; } = CircleStatusEnum.Active;

    public int OrganizationId { get; set; }
    public Organization Organization { get; set; } = null!;
    public virtual ICollection<CircleMember> CircleMembers { get; set; } = null!;
}

/*

members(owner, )
posts
tags: circle, tags: asp.net core, mssql, vue
proposal
*/

/* other

Description: very short keywords about the project(public)
Summary: short notes about the idea (IsVisiable = false) exceptions: invitaions, members, staff 
tags: circle, tags: asp.net core, mssql, vue,
links
memeber view:
    owner, 
    supervisors, 
    students

posts: public, private

*/

/*
Proposal: PDF Reader,
Sessions: Cards:   5 sessions
    done, 
    doing,
    to do, 
    ext session date, 
 
- Rate(Archive)
- 

*/