using TeamsMaker.Api.Contracts.Requests.Profile;

namespace TeamsMaker.Api.Services.Profiles.Interfaces;

public interface IProjectService
{
    Task AddProjectAsync(AddProjectRequest request, CancellationToken ct);
    Task UpdateProjectAsync(int projectId, AddProjectRequest request, CancellationToken ct);
    Task DeleteProjectAsync(int projectId, CancellationToken ct);
}

