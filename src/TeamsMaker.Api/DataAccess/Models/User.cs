using TeamsMaker.Api.DataAccess.Base.Interfaces;
using TeamsMaker.Core.Enums;

namespace TeamsMaker.Api.DataAccess.Models;


public class User : IdentityUser, IReadOnlyOrganizationInfo, IActivable
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string SSN { get; set; } = string.Empty;
    public byte[]? Avatar { get; set; }
    public byte[]? Header { get; set; }
    public string? Bio { get; set; }
    public string? About { get; set; }
    public int Gender { get; set; } = (int) GenderEnum.Unknown;
    public string? City { get; set; }
    public bool IsActive { get; set; } = true;
    public int OrganizationId { get; set; }
    public Organization Organization { get; set; } = null!;
    // TODO: Dept
    public ICollection<RefreshToken>? RefreshTokens { get; set; }
}
//TODO: Department, Contacts, Circle, Posts, UserLinks, Tags