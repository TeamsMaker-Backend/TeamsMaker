using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Contracts.Requests.Profile;
using TeamsMaker.Api.Contracts.Responses.Profile;

namespace TeamsMaker.Api.Services.Profiles.Interfaces;

public interface IProfileService
{
    Task<GetProfileResponse> GetProfileAsync(CancellationToken ct);
    Task<GetOtherProfileResponse> GetOtherProfileAsync(string id,CancellationToken ct);
    Task<FileContentResult?> GetFileContentAsync(Guid id, string fileType, CancellationToken ct);
    Task UpdateProfileAsync(UpdateProfileRequest profileRequest, CancellationToken ct);
}
