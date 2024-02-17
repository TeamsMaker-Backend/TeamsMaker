using Core.ValueObjects;

using TeamsMaker.Api.DataAccess.Base;
using TeamsMaker.Api.DataAccess.Base.Interfaces;

namespace TeamsMaker.Api.DataAccess.Models;

public class Organization : TrackedEntity<int>, IActivable
{
    public TranslatableString Name { get; set; } = TranslatableString.Empty;
    public string Address { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Logo { get; set; } //TODO: string url
    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;

    public virtual ICollection<User> Users { get; set; } = [];
    public virtual ICollection<Role> Roles { get; set; } = [];
    public virtual ICollection<Department> Departments { get; set; } = [];
}
