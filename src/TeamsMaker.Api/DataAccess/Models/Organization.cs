using Core.ValueObjects;

using DataAccess.Base;
using DataAccess.Base.Interfaces;

namespace TeamsMaker.Api.DataAccess.Models;

public class Organization : TrackedEntity<int>, IActivable
{
    public TranslatableString Name { get; set; } = TranslatableString.Empty;
    public string Address { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public byte[]? Logo { get; set; }
    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public ICollection<User>? Users { get; set; }
    public ICollection<Role>? Roles { get; set; }
}
