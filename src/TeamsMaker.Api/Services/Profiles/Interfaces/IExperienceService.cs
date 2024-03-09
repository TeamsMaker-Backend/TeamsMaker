using Microsoft.AspNetCore.JsonPatch;

using TeamsMaker.Api.Contracts.Requests.Profile;

namespace TeamsMaker.Api.Services.Profiles.Interfaces;

public interface IExperienceService
{
    Task AddExperienceAsync(AddExperienceRequest request, CancellationToken ct);
    Task UpdateExperienceAsync(int experienceId, UpdateExperienceRequest updateExperienceRequest, CancellationToken ct);
    Task DeleteExperienceAsync(int experienceId, CancellationToken ct);
}