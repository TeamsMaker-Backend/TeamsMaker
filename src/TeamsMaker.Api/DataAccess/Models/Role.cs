using TeamsMaker.Api.DataAccess.Base.Interfaces;

namespace TeamsMaker.Api.DataAccess.Models;

public class Role : IdentityRole, IActivable
{
    public bool IsOrganizationAdmin { get; set; }
    public bool IsActive { get; set; } = true;
    public int OrganizationId { get; set; }

    public virtual Organization Organization { get; set; } = null!;
}