using TeamsMaker.Api.Contracts.Requests.Profile;
using TeamsMaker.Api.Contracts.Responses.Profile;

namespace TeamsMaker.Api.Services.Profiles.Interfaces;

public interface IProfileService
{
    Task<GetProfileResponse> GetAsync(CancellationToken ct);
    Task<GetOtherProfileResponse> GetOtherAsync(string id, CancellationToken ct);
    Task UpdateAsync(UpdateProfileRequest profileRequest, CancellationToken ct);
}
