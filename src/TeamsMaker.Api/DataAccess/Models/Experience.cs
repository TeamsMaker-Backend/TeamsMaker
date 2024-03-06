using TeamsMaker.Api.DataAccess.Base;

namespace TeamsMaker.Api.DataAccess.Models;

public class Experience : BaseEntity<int>
{
    public string Title { get; set; } = null!;
    public string Organization { get; set; } = null!;
    public string Role { get; set; } = null!;
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public string? Description { get; set; }

    public string StudentId { get; set; } = null!;
    public virtual Student Student { get; set; } = null!;
}