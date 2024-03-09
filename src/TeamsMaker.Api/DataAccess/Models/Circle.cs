using Core.ValueObjects;

using TeamsMaker.Api.DataAccess.Base;
using TeamsMaker.Api.DataAccess.Base.Interfaces;
using TeamsMaker.Core.Enums;

namespace TeamsMaker.Api.DataAccess.Models;

public class Circle : TrackedEntity<Guid>, IReadOnlyOrganizationInfo
{
    public string Name { get; set; } = null!;
    public string? About { get; set; }
    public SummaryData? Summary { get; set; }
    public FileData? Avatar { get; set; }
    public FileData? Header { get; set; } // back ground
    public long Rate { get; set; }
    public CircleStatusEnum Status { get; set; } = CircleStatusEnum.Active;

    public int OrganizationId { get; set; }
    public Organization Organization { get; set; } = null!;
    public virtual ICollection<CircleMember> CircleMembers { get; set; } = null!;
    public virtual ICollection<Link> Links { get; set; } = [];
    public virtual ICollection<Skill> Skills { get; set; } = []; // tech stack
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
    - posts
*/