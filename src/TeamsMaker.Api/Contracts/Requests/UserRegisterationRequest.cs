namespace TeamsMaker.Api.Contracts.Requests;

public class UserRegisterationRequest
{
    [Required]
    public required string UserName { get; init; }

    [EmailAddress, Required]
    public required string Email { get; init; }

    [Required]
    public required string Password { get; init; }
}
