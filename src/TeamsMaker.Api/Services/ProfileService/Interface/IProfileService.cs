using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Contracts.Requests.Profile;
using TeamsMaker.Api.Contracts.Responses.Profile;

namespace TeamsMaker.Api.Services.ProfileService.Interface;

public interface IProfileService
{
    Task<ProfileResponse> GetProfileAsync(CancellationToken ct);
    Task<FileContentResult?> GetAvatarAsync(Guid id, CancellationToken ct);
    Task<FileContentResult?> GetHeaderAsync(Guid id, CancellationToken ct);
    Task<FileContentResult?> GetCVAsync(Guid id, CancellationToken ct);
    Task UpdateProfileAsync(UpdateProfileRequest profileRequest, CancellationToken ct);
}
