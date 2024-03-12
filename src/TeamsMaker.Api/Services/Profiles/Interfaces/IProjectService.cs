using TeamsMaker.Api.Contracts.Requests.Profile;

namespace TeamsMaker.Api.Services.Profiles.Interfaces;

public interface IProjectService
{
    Task AddAsync(AddProjectRequest request, CancellationToken ct);
    Task UpdateAsync(int projectId, AddProjectRequest request, CancellationToken ct);
    Task DeleteAsync(int projectId, CancellationToken ct);
}

