using TeamsMaker.Api.DataAccess.Base;

namespace TeamsMaker.Api.DataAccess.Models;

public class Session : TrackedEntity<Guid>
{
    public string Title { get; set; } = null!;
    public string? Notes { get; set; }
    public SessionStatus Status { get; set; } = SessionStatus.Upcoming;
    public DateOnly Date { get; set; }
    public TimeOnly Time { get; set; }

    public virtual ICollection<TodoTask> TodoTasks { get; set; } = [];

    public Guid? CircleId { get; set; }
    public virtual Circle? Circle { get; set; }
}
