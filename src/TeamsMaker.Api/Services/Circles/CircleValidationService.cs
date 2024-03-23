using TeamsMaker.Api.Core.Enums;
using TeamsMaker.Api.DataAccess.Context;
using TeamsMaker.Api.Services.Circles.Interfaces;

namespace TeamsMaker.Api.Services.Circles;

public class CircleValidationService(AppDBContext db) : ICircleValidationService
{
    public async Task<CircleMember> TryGetOwnerAsync(string userId, Guid circleId, CancellationToken ct)
    {
        var circleMember = await TryGetCircleMemberAsync(userId, circleId, ct);

        return !circleMember.IsOwner ? throw new ArgumentException("Not The Circle Owner") : circleMember;
    }

    public async Task<CircleMember> TryGetCircleMemberAsync(string userId, Guid circleId, CancellationToken ct)
    {
        var circleMember = await db.CircleMembers
            .Include(cm => cm.ExceptionPermission)
            .SingleOrDefaultAsync(cm => cm.UserId == userId && cm.CircleId == circleId, ct) ??
            throw new ArgumentException("Not a Circle Member");

        return circleMember;
    }

    public bool HasPermission(CircleMember circleMember, Circle circle, PermissionsEnum permission)
    {
        if (circleMember.IsOwner)
            return true;

        if (circleMember.ExceptionPermission is not null &&
            !IsOn(circleMember.ExceptionPermission, permission))
            throw new ArgumentException("Donot Have The Permission");

        if (!IsOn(circle.DefaultPermission, permission))
            throw new ArgumentException("Donot Have The Permission");

        return true;
    }

    public bool IsOn(Permission userPermission, PermissionsEnum permissionToCheck)
        => permissionToCheck switch
        {
            PermissionsEnum.CircleManagement => userPermission.CircleManagment,
            PermissionsEnum.MemberManagement => userPermission.MemberManagement,
            PermissionsEnum.ProposalManagement => userPermission.ProposalManagment,
            PermissionsEnum.FeedManagement => userPermission.FeedManagment,
            _ => false,
        };
}
