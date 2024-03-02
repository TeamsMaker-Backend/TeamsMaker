using TeamsMaker.Api.DataAccess.Base;

namespace TeamsMaker.Api.DataAccess.Models;

public class MemberRole : BaseEntity<Guid>
{
    public Guid CircleRoleId { get; set; }
    public Guid CircleMemberId { get; set; }
    
    public virtual CircleRole CircleRole { get; set; } = null!;
    public virtual CircleMember CircleMember { get; set; } = null!;
}
