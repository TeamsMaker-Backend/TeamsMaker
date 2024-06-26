﻿using TeamsMaker.Api.DataAccess.Base;

namespace TeamsMaker.Api.DataAccess.Models;

public class Project : BaseEntity<int>
{
    public string Name { get; set; } = null!;
    public string? Url { get; set; }
    public string? Description { get; set; }
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public ICollection<Skill> Skills { get; set; } = [];

    public string StudentId { get; set; } = null!;
    public virtual Student Student { get; set; } = null!;
}