using System.ComponentModel.DataAnnotations;

namespace TeamsMaker.Api;

public class UserLoginRequest
{
    [EmailAddress, Required]
    public required string Email { get; init; }

    [Required]
    public required string Password { get; init; }
}
