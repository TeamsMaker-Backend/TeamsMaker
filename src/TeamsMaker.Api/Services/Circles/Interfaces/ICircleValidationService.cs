using TeamsMaker.Api.Core.Enums;

namespace TeamsMaker.Api.Services.Circles.Interfaces;

public interface ICircleValidationService
{
    Task<CircleMember> TryGetOwnerAsync(string userId, Guid circleId, CancellationToken ct);
    Task<CircleMember> TryGetCircleMemberAsync(string userId, Guid circleId, CancellationToken ct);
    void CheckPermission(CircleMember circleMember, Circle circle, PermissionsEnum permissions);
    bool HasPermission(CircleMember circleMember, Circle circle, PermissionsEnum permissions);
    bool IsOn(Permission userPermission, PermissionsEnum permissionToCheck);
}
