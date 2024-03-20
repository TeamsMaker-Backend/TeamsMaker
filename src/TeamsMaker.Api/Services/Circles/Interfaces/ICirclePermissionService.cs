using TeamsMaker.Api.Core.Enums;

namespace TeamsMaker.Api.Services.Circles.Interfaces;

public interface ICirclePermissionService
{
    Task<CircleMember> TryGetOwnerAsync(string userId, Guid circleId, CancellationToken ct);
    Task<CircleMember> TryGetCircleMemberAsync(string userId, Guid circleId, CancellationToken ct);
    bool HasPermission(CircleMember circleMember, Circle circle, PermissionsEnum permissions);
    bool IsOn(Permission userPermission, PermissionsEnum permissionToCheck);
}
