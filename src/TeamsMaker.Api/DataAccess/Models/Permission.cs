using TeamsMaker.Api.DataAccess.Base;

namespace TeamsMaker.Api.DataAccess.Models;

public class Permission : BaseEntity<int>
{
    public string Name { get; set; } = null!;

    public virtual ICollection<MemberPermission> MemberPermissions { get; set; } = [];
}
