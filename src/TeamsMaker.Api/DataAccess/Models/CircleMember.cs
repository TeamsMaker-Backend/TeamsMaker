using TeamsMaker.Api.DataAccess.Base;

namespace TeamsMaker.Api.DataAccess.Models;

public class CircleMember : BaseEntity<Guid>
{
    public bool IsOwner { get; set; }
    public Guid UserId { get; set; }
    public Guid CircleId { get; set; }
    public virtual User User { get; set; } = null!;
    public virtual Circle Circle { get; set; } = null!;
    public virtual ICollection<MemberRole> MemberRoles { get; set; } = [];
}
