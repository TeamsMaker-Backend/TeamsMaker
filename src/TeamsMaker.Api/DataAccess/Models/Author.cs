using TeamsMaker.Api.DataAccess.Base;

namespace TeamsMaker.Api.DataAccess.Models;

public class Author : BaseEntity<Guid>
{
    public string? UserId { get; set; }
    public Guid? CircleId { get; set; }

    public virtual User? User { get; set; }
    public virtual Circle? Circle { get; set; }
    public virtual ICollection<Post> Posts { get; set; } = [];
}
