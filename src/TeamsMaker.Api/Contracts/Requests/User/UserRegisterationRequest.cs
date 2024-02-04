namespace TeamsMaker.Api.Contracts.Requests;

public class UserRegisterationRequest
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string UserName { get; init; }
    [EmailAddress] public required string Email { get; init; }
    public required string Password { get; init; }
}
