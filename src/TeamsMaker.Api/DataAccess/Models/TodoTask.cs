using TeamsMaker.Api.DataAccess.Base;

namespace TeamsMaker.Api.DataAccess.Models;

public class TodoTask : TrackedEntity<Guid>
{
    public string Title { get; set; } = null!;
    public string? Notes { get; set; }
    public TodoTaskStatus Status { get; set; } = TodoTaskStatus.NotStarted;
    public DateOnly DeadLine { get; set; }

    public Guid? SessionId { get; set; }
    public virtual Session? Session { get; set; }

    public Guid? CircleId { get; set; }
    public virtual Circle? Circle { get; set; }
}
