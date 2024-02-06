using TeamsMaker.Api.DataAccess.Base.Interfaces;
using TeamsMaker.Core.Enums;

namespace TeamsMaker.Api.DataAccess.Models;


public class User : IdentityUser, IActivable
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string SSN { get; set; } = string.Empty;
    public string? Avatar { get; set; }
    public string? Header { get; set; }
    public string? Bio { get; set; }
    public string? About { get; set; }
    public GenderEnum Gender { get; set; } = GenderEnum.Unknown;
    public string? City { get; set; }
    public bool IsActive { get; set; } = true;
    public int OrganizationId { get; set; }
    public virtual Organization Organization { get; set; } = null!;
    // TODO: Dept
    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = [];

    // public static User Create(string firstName, string lastName, string email, string userName)
    // {
    //     return new User
    //     {
    //         FirstName = firstName,
    //         LastName = lastName,
    //         Email = email,
    //         UserName = userName
    //     };
    // }


    public User WithOrganizationId(int organizationId)
    {
        OrganizationId = organizationId;

        return this;
    }

    public static implicit operator User(bool v)
    {
        throw new NotImplementedException();
    }

}
//TODO: Department, Contacts, Circle, Posts, UserLinks, Tags