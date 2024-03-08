using TeamsMaker.Api.DataAccess.Base;

namespace TeamsMaker.Api.DataAccess.Models;

public class CircleMember : BaseEntity<Guid>
{
    public string UserId { get; set; } = null!;
    public Guid CircleId { get; set; }
    public bool IsOwner { get; set; }

    public virtual User User { get; set; } = null!;
    public virtual Circle Circle { get; set; } = null!;
    public virtual ICollection<MemberPermission> MemberPermissions { get; set; } = [];
}
//TODO: title