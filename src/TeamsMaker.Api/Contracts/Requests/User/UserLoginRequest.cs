using System.ComponentModel.DataAnnotations;

namespace TeamsMaker.Api;

public class UserLoginRequest
{
    [EmailAddress] public required string Email { get; init; }
    public required string Password { get; init; }
}
