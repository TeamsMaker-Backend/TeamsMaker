﻿using TeamsMaker.Api.DataAccess.Base;

namespace TeamsMaker.Api.DataAccess.Models;

//TODO: refacor the double relationship
public class Skill : BaseEntity<int>
{
    public string Name { get; set; } = null!;
    public int? ProjectId { get; set; }
    public Guid? CircleId { get; set; }

    public virtual Project? Project { get; set; }
    public virtual Circle? Circle { get; set; }
}