using TeamsMaker.Api.DataAccess.Base;
using TeamsMaker.Api.DataAccess.Base.Interfaces;

namespace TeamsMaker.Api.DataAccess.Models;

public class Department : TrackedEntity<int>, IOrganizationInfo, IActivable
{
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;

    public bool IsActive { get; set; } = true;

    public int OrganizationId { get; set; }

    public virtual Organization Organization { get; set; } = null!;
    public virtual ICollection<DepartmentStaff> DepartmentStaff { get; set; } = [];
    public virtual ICollection<Student> Students { get; set; } = [];
}
