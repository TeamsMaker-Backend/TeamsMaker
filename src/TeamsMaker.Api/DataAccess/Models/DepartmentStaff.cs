using TeamsMaker.Api.DataAccess.Base;

namespace TeamsMaker.Api.DataAccess.Models;

public class DepartmentStaff : TrackedEntity<Guid>
{
    public bool IsHead { get; set; }
    public int DepartmentId { get; set; }
    public Department Department { get; set; } = null!;

    public string StaffId { get; set; } = null!;
    public Staff Staff { get; set; } = null!;
}
