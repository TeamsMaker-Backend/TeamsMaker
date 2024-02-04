using TeamsMaker.Api.Contracts.Requests;
using TeamsMaker.Api.Contracts.Responses;

namespace TeamsMaker.Api.Services.Auth;

public interface IAuthService
{
    Task<TokenResponse> LoginAsync(UserLoginRequest loginRequest, CancellationToken ct);
    Task<TokenResponse> RegisterAsync(UserRegisterationRequest registerationRequest, CancellationToken ct);
    Task<TokenResponse> RefreshTokenAsync(TokenRequest tokenRequest, CancellationToken ct);
    Task<bool> VerifyUserAsync(UserVerificationRequset verificationRequest, CancellationToken ct);
}
