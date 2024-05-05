using TeamsMaker.Core.Enums;

namespace TeamsMaker.Api.Contracts.Responses.Profile;

public class GetUserAsRowResponse
{
    public string Id { get; set; } = null!;
    public string? Avatar { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? Bio { get; set; }
    public UserEnum? UserType { get; set; }
}