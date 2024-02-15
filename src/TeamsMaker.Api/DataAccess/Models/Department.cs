using TeamsMaker.Api.DataAccess.Base;
using TeamsMaker.Api.DataAccess.Base.Interfaces;

namespace TeamsMaker.Api.DataAccess.Models;

public class Department : TrackedEntity<int>, IActivable
{
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;

    public bool IsActive { get; set; } = true;

    public virtual ICollection<DepartmentStaff> DepartmentStaff { get; set; } = [];
    public virtual ICollection<Student> Students { get; set; } = [];
}
