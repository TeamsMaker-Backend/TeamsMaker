﻿using TeamsMaker.Api.DataAccess.Base;

namespace TeamsMaker.Api.DataAccess.Models;

public class CircleMember : BaseEntity<Guid>
{
    public bool IsOwner { get; set; } = false;
    public string? Badge { get; set; } //TODO: MemeberBadges const
    public string? Role { get; set; }
    public bool IsSupervisor { get; set; }
    public bool IsCosupervisor { get; set; }

    public virtual Permission? ExceptionPermission { get; set; }
    public string UserId { get; set; } = null!;
    public virtual User User { get; set; } = null!;
    public Guid CircleId { get; set; }
    public virtual Circle Circle { get; set; } = null!;
}
//TODO: roles