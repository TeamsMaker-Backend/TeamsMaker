using TeamsMaker.Api.Contracts.Requests.Profile;

namespace TeamsMaker.Api.Services.Profiles.Interfaces
{
    public interface IProjectService
    {
        Task AddProjectAsync(ProjectRequest request, CancellationToken ct);
        Task UpdateProjectAsync(int projectId, ProjectRequest request, CancellationToken ct);
        Task DeleteProjectAsync(int projectId, CancellationToken ct);
    }
}
