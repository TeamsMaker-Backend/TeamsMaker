using TeamsMaker.Api.Contracts.Requests.Profile;
using TeamsMaker.Api.Contracts.Responses.Profile;

namespace TeamsMaker.Api.Services.ProfileService.Interface;

public interface IProfileService
{
    Task<ProfileResponse> GetProfileAsync(string email, CancellationToken ct);
    Task UpdateProfileAsync(Guid id, UpdateProfileRequest profileRequest, CancellationToken ct);
}
