using Core.ValueObjects;

using TeamsMaker.Api.DataAccess.Base;
using TeamsMaker.Api.DataAccess.Base.Interfaces;
using TeamsMaker.Core.Enums;

namespace TeamsMaker.Api.DataAccess.Models;

public class Circle : TrackedEntity<Guid>, IReadOnlyOrganizationInfo
{
    public string Name { get; set; } = null!;
    public string? Keywords { get; set; }
    public SummaryData? SummaryData { get; set; }
    public FileData? Avatar { get; set; }
    public FileData? Header { get; set; } // back ground
    public long Rate { get; set; }
    public CircleStatusEnum Status { get; set; } = CircleStatusEnum.Active;
    public DateTime? ArchivedOn { get; set; }

    public int OrganizationId { get; set; }
    public virtual Organization Organization { get; set; } = null!;
    public virtual Permission DefaultPermission { get; set; } = null!;
    public virtual Proposal? Proposal { get; set; }
    public virtual Author? Author { get; set; }
    public virtual ICollection<CircleMember> CircleMembers { get; set; } = null!;
    public virtual ICollection<Link> Links { get; set; } = [];
    public virtual ICollection<Skill> Skills { get; set; } = []; // tech stack
    public virtual ICollection<JoinRequest> Invitions { get; set; } = []; // tech stack
    public virtual ICollection<Session> Sessions { get; set; } = [];
    public virtual ICollection<TodoTask> TodoTasks { get; set; } = [];
    public virtual ICollection<Upvote> Upvotes { get; set; } = [];
}
