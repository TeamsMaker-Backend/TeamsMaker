using TeamsMaker.Api.DataAccess.Base;

namespace TeamsMaker.Api.DataAccess.Models;

public class CircleRole : BaseEntity<Guid>
{

    public virtual ICollection<MemberRole> MemberRoles { get; set; } = [];
}
