using Core.ValueObjects;

using TeamsMaker.Api.DataAccess.Base;
using TeamsMaker.Api.DataAccess.Base.Interfaces;
using TeamsMaker.Core.Enums;

namespace TeamsMaker.Api.DataAccess.Models;

public class Circle : TrackedEntity<Guid>
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public FileData? Avatar { get; set; }
    public FileData? Header { get; set; } // back ground
    public CircleStatusEnum Status { get; set; } = CircleStatusEnum.Active;

    public virtual ICollection<CircleMember> CircleMembers { get; set; } = null!;
}
