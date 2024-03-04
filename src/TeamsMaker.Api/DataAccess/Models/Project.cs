using TeamsMaker.Api.DataAccess.Base;

namespace TeamsMaker.Api.DataAccess.Models;

public class Project : BaseEntity<int>
{
    public string Name { get; set; } = null!;
    public string Url { get; set; } = null!;
    public string? Description { get; set; }
    public ICollection<Skill> Skills { get; set; } = [];

    public string StudentId { get; set; } = null!;
    public virtual Student Student { get; set; } = null!;
}