using TeamsMaker.Api.Contracts.Requests.Profile;

namespace TeamsMaker.Api.Services.Profiles.Interfaces;

public interface IStudentProfileService
{
    Task AddProjectAsync(ProjectRequest request, CancellationToken ct);
    Task UpdateProjectAsync(int projectId, ProjectRequest request, CancellationToken ct);
    Task DeleteProjectAsync(int projectId, CancellationToken ct);

    Task AddExperienceAsync(ExperienceRequest request, CancellationToken ct);
    Task UpdateExperienceAsync(int experienceId, ExperienceRequest request, CancellationToken ct);
    Task DeleteExperienceAsync(int experienceId, CancellationToken ct);
}
