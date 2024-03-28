using TeamsMaker.Api.DataAccess.Base;

namespace TeamsMaker.Api.DataAccess.Models;

public class CircleMember : BaseEntity<Guid>
{
    public bool IsOwner { get; set; } = false;
    public string? Badge { get; set; } // add all padges as a string, sepreated by comma ','

    public virtual Permission? ExceptionPermission { get; set; }
    public string UserId { get; set; } = null!;
    public virtual User User { get; set; } = null!;
    public Guid CircleId { get; set; }
    public virtual Circle Circle { get; set; } = null!;
}
//TODO: title