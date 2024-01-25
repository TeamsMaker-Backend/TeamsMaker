using TeamsMaker.Api.Contracts.Requests;

namespace TeamsMaker.Api;

public interface IAuthService
{
    Task LoginAsync(UserLoginRequest loginRequest, CancellationToken ct);
    Task RegisterAsync(UserRegisterationRequest registerationRequest, CancellationToken ct);
    Task<bool> VerifyUserAsync(UserVerificationRequset verificationRequest, CancellationToken ct);
}
