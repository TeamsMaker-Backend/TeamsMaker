namespace TeamsMaker.Api.DataAccess.Models;

public class Staff : User
{
    public StaffClassificationsEnum Classification { get; set; }

    public virtual ICollection<DepartmentStaff> DepartmentStaff { get; set; } = [];
    public virtual ICollection<ApprovalRequest> ApprovalRequests { get; set; } = [];
    public virtual ICollection<ApprovalRequest> AcceptedApprovalRequests { get; set; } = [];
}

//TODO: public int Creds { get; set; }
//TODO: Circles
//TODO: Proposals