using TeamsMaker.Api.Contracts.Requests.Profile;

namespace TeamsMaker.Api.Services.Profiles.Interfaces;

public interface IExperienceService
{
    Task AddAsync(AddExperienceRequest request, CancellationToken ct);
    Task UpdateAsync(int experienceId, UpdateExperienceRequest updateExperienceRequest, CancellationToken ct);
    Task DeleteAsync(int experienceId, CancellationToken ct);
}