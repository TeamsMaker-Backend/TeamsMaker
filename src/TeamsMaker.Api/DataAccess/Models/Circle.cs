using Core.ValueObjects;

using TeamsMaker.Api.DataAccess.Base;
using TeamsMaker.Api.DataAccess.Base.Interfaces;

namespace TeamsMaker.Api.DataAccess.Models;

public class Circle : TrackedEntity<Guid>, IActivable
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public FileData? Avatar { get; set; }
    public FileData? Header { get; set; } // back ground
    public virtual ICollection<CircleMember> CircleMembers { get; set; } = null!;

    public bool IsActive { get; set; }
}
