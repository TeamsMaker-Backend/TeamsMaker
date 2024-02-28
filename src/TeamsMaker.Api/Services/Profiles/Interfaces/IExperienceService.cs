using TeamsMaker.Api.Contracts.Requests.Profile;

namespace TeamsMaker.Api.Services.Profiles.Interfaces
{
    public interface IExperienceService
    {
        Task AddExperienceAsync(ExperienceRequest request, CancellationToken ct);
        Task UpdateExperienceAsync(int experienceId, ExperienceRequest request, CancellationToken ct);
        Task DeleteExperienceAsync(int experienceId, CancellationToken ct);
    }
}
