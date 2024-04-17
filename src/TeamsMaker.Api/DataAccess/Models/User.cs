using Core.ValueObjects;

using TeamsMaker.Api.DataAccess.Base.Interfaces;
using TeamsMaker.Core.Enums;

namespace TeamsMaker.Api.DataAccess.Models;


public class User : IdentityUser, IActivable
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string SSN { get; set; } = null!;
    public FileData? Avatar { get; set; }
    public FileData? Header { get; set; }
    public string? Bio { get; set; }
    public string? About { get; set; }
    public GenderEnum Gender { get; set; } = GenderEnum.Unknown;
    public string? City { get; set; }
    public bool IsActive { get; set; } = true;
    public int OrganizationId { get; set; }

    public virtual Organization Organization { get; set; } = null!;
    public virtual Author? Author { get; set; }
    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = [];
    public virtual ICollection<Link> Links { get; set; } = [];
    public virtual ICollection<CircleMember> MemberOn { get; set; } = [];
    public virtual ICollection<Upvote> Upvotes { get; set; } = [];
}
//TODO: Contacts, Circle, Posts, UserLinks, Tags