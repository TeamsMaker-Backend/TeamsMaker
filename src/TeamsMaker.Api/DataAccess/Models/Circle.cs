using Core.ValueObjects;

using TeamsMaker.Api.DataAccess.Base;
using TeamsMaker.Api.DataAccess.Base.Interfaces;
using TeamsMaker.Core.Enums;

namespace TeamsMaker.Api.DataAccess.Models;

public class Circle : TrackedEntity<Guid>, IReadOnlyOrganizationInfo
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public SummaryData? Summary { get; set; }
    public FileData? Avatar { get; set; }
    public FileData? Header { get; set; } // back ground
    public long Rate { get; set; }
    public CircleStatusEnum Status { get; set; } = CircleStatusEnum.Active;

    public Permission DefaultPermission { get; set; } = null!;
    public int OrganizationId { get; set; }
    public virtual Organization Organization { get; set; } = null!;
    public virtual ICollection<CircleMember> CircleMembers { get; set; } = null!;
    public virtual ICollection<Link> Links { get; set; } = [];
    public virtual ICollection<Skill> Skills { get; set; } = []; // tech stack
    public virtual ICollection<JoinRequest> Invitions { get; set; } = []; // tech stack
    public virtual ICollection<Session> Sessions { get; set; } = [];
    public virtual ICollection<TodoTask> TodoTasks { get; set; } = [];
}

/*

circle
    - avatar/header/name/bio(short key words about the project) 
    - join or invite 
    - settings  
    - indicators: active/archived/ upvote 
    - member view -> with special page to view them whith badges
    - tech stack
    - summary (public/private)
    - communciation chennels (telegram/whatsup/github/discord)
    - sessions (todo, done, next)
    - Proposal link 
        - Form : (circle name, overview, objectives, tech stack )
        - Approvals (name of doctor)
    - posts(public)
*/

/*

1- Add Circle (student just a one circle, prof) , assign ownership permissions
2- Update (info)
3- Update files (fileService)
4- Get => member , other, (other but invited ?? privacy issue)
5- get circle members with details: name, badges
6- Delete, Archive (owner, ??), Transfer ownership
7- settings --
8- proposal: to be continued

*/

/*
    1- Add circle.
    2- update (info), (media), (privacy), (communication channels)
    3- Add invitions.
    4- Accept requests.
    5- Remove members.
    6- update permissions { grouped one, exception one }.
    7- Get Circle (CircleMember), (Other), (other but invited ?? privacy issue)
    8- Get Circle Members
 */

/*
    Khalid: 2, 7

    El-Saghir: 1, 8

    Abdo: 3, 4, 5

    Sa3d: 6
 */
