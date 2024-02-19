namespace TeamsMaker.Api.Contracts.Requests;

public class TokenRequest
{
    [Required]
    public string Token { get; init; } = null!;

    [Required]
    public string RefreshToken { get; init; } = null!;
}
