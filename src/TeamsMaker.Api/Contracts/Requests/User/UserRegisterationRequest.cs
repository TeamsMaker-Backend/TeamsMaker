using TeamsMaker.Core.Enums;

namespace TeamsMaker.Api.Contracts.Requests;

public class UserRegisterationRequest
{
    public UserEnum UserType { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    [EmailAddress] public required string Email { get; init; }
    public required string Password { get; init; }
    public required string SSN { get; init; }
}
