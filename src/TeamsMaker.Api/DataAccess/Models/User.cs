using TeamsMaker.Api.DataAccess.Base.Interfaces;
using TeamsMaker.Core.Enums;

namespace TeamsMaker.Api.DataAccess.Models;


public class User : IdentityUser, IActivable
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string SSN { get; set; } = string.Empty;
    public string? Avatar { get; set; }
    public string? Header { get; set; }
    public string? Bio { get; set; }
    public string? About { get; set; }
    public int Gender { get; set; } = (int)GenderEnum.Unknown;
    public string? City { get; set; }
    public bool IsActive { get; set; } = true;
    public int OrganizationId { get; set; }
    public virtual Organization Organization { get; set; } = null!;
    // TODO: Dept
    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = [];
}
//TODO: Department, Contacts, Circle, Posts, UserLinks, Tags