using TeamsMaker.Api.DataAccess.Base;
using TeamsMaker.Api.DataAccess.Base.Interfaces;

namespace TeamsMaker.Api.DataAccess.Models;

public class MemberPermission : BaseEntity<Guid>, IActivable
{
    public bool IsActive { get; set; }

    public int PermissionId { get; set; }
    public Guid CircleMemberId { get; set; }
    
    public virtual Permission Permission { get; set; } = null!;
    public virtual CircleMember CircleMember { get; set; } = null!;
}
