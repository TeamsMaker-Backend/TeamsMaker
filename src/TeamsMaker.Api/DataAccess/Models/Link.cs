﻿using TeamsMaker.Api.Core.Enums;
using TeamsMaker.Api.DataAccess.Base;

namespace TeamsMaker.Api.DataAccess.Models;

//TODO: refacor the double relationship
public class Link : BaseEntity<int>
{
    public string Url { get; set; } = null!;
    public LinkTypesEnum Type { get; set; }

    public string? UserId { get; set; }
    public Guid? CircleId { get; set; }

    public virtual User? User { get; set; }
    public virtual Circle? Circle { get; set; }
}
