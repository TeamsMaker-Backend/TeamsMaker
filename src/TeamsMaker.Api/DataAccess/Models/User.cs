using DataAccess.Base.Interfaces;

using TeamsMaker.Core.Enums;

namespace TeamsMaker.Api.DataAccess.Models;


public class User : IdentityUser<Ulid>, IOrganizationInfo, IActivable
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string SSN { get; set; } = string.Empty;
    public string CollegeId { get; set; } = string.Empty;
    public float GPA { get; set; }
    public byte[]? Avatar { get; set; }
    public byte[]? Header { get; set; }
    public string? Bio { get; set; }
    public string? About { get; set; }
    public DateOnly GraduationYear { get; set; }
    public int Level { get; set; }
    public byte[]? CV { get; set; }
    public GenderEnum Gender { get; set; } // enum Male = 1, Female = 2 
    public string? City { get; set; }

    public bool IsActive { get; set; }
    public int OrganizationId { get; set; }
    public Organization Organization { get; set; } = null!;
    public ICollection<RefreshToken>? RefreshTokens { get; set; }

}
//TODO: Department, Contacts, Circle, Posts, UserLinks, Tags

// profile name first second,