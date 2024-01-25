using DataAccess.Base.Interfaces;

namespace TeamsMaker.Api.DataAccess.Models;

public class Role : IdentityRole<Ulid>, IActivable, IReadOnlyOrganizationInfo
{
    public bool IsOrganizationAdmin { get; set; }
    public bool IsActive { get; set; }
    public int OrganizationId { get; set; }

    public Organization Organization { get; set; } = null!;
}