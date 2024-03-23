using TeamsMaker.Api.Contracts.Requests.Circle;

namespace TeamsMaker.Api.Services.Circles.Interfaces;

internal interface IPermissionService
{
    Task UpdatePermissionAsync(Guid entityId, UpdatePermissionRequest? request, CancellationToken ct);
}