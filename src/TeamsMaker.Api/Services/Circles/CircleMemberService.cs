using DataAccess.Base.Interfaces;

using TeamsMaker.Api.Contracts.Requests.Circle;
using TeamsMaker.Api.Core.Enums;
using TeamsMaker.Api.DataAccess.Context;
using TeamsMaker.Api.Services.Circles.Interfaces;

namespace TeamsMaker.Api.Services.Circles;

public class CircleMemberService
    (AppDBContext db, IUserInfo userInfo, ICircleValidationService validationService) : ICircleMemberService, IPermissionService
{
    public async Task AddAsync(Guid circleId, String userId, CancellationToken ct)
    {
        if (!await db.Users.AnyAsync(s => s.Id == userId, ct))
            throw new ArgumentException("Invalid Student Id");

        var circleMember = await validationService.TryGetCircleMemberAsync(userInfo.UserId, circleId, ct);

        var circle = await db.Circles
            .Include(c => c.DefaultPermission)
            .Include(c => c.CircleMembers)
            .SingleOrDefaultAsync(c => c.Id == circleId, ct) ??
            throw new ArgumentException("Invalid Circle Id");

        validationService.CheckPermission(circleMember, circle, PermissionsEnum.MemberManagement);

        if (await db.CircleMembers.AnyAsync(cm => cm.UserId == userId, ct))
            throw new ArgumentException("Student Cannot Be In Two Circles");

        circle.CircleMembers.Add(new CircleMember
        {
            UserId = userId
        });

        await db.SaveChangesAsync(ct);
    }

    public async Task RemoveAsync(Guid circleMemberId, CancellationToken ct)
    {
        var removedCircleMember = await db.CircleMembers
            .FindAsync([circleMemberId], ct) ??
            throw new ArgumentException("Not a circle Member");

        var currentCircleMember = await validationService.TryGetCircleMemberAsync(userInfo.UserId, removedCircleMember.CircleId, ct);

        var circle = await db.Circles
            .Include(c => c.DefaultPermission)
            .SingleOrDefaultAsync(c => c.Id == removedCircleMember.CircleId, ct) ??
            throw new ArgumentException("Invalid Circle ID");

        if (userInfo.UserId != removedCircleMember.UserId) // leaves (removes himself)
            validationService.CheckPermission(currentCircleMember, circle, PermissionsEnum.MemberManagement);

        db.CircleMembers.Remove(removedCircleMember);

        await db.SaveChangesAsync(ct);
    }

    public async Task UpdateBadgeAsync(Guid circleMemberId, string? badge, CancellationToken ct)
    {
        var updatedCircleMember = await db.CircleMembers
            .FindAsync([circleMemberId], ct) ??
            throw new ArgumentException("Not a circle Member");

        var currentCircleMember = await validationService.TryGetCircleMemberAsync(userInfo.UserId, updatedCircleMember.CircleId, ct);

        var circle = await db.Circles
            .Include(c => c.DefaultPermission)
            .SingleOrDefaultAsync(c => c.Id == updatedCircleMember.CircleId, ct) ??
            throw new ArgumentException("Invalid Circle ID");

        validationService.CheckPermission(currentCircleMember, circle, PermissionsEnum.MemberManagement);

        updatedCircleMember.Badge = badge; // Todo: string splitting ','

        await db.SaveChangesAsync(ct);
    }

    public async Task UpdatePermissionAsync(Guid circleMemberId, UpdatePermissionRequest? request, CancellationToken ct)
    {
        var circleMember = await db.CircleMembers
            .FindAsync([circleMemberId], ct) ??
            throw new ArgumentException("Not a circle Member");

        if (circleMember.IsOwner)
            throw new ArgumentException("Cannot Change Circle Owner Permissions");

        await validationService.TryGetOwnerAsync(userInfo.UserId, circleMember.CircleId, ct);

        if (circleMember.ExceptionPermission is not null)
            db.Permissions.Remove(circleMember.ExceptionPermission);

        circleMember.ExceptionPermission = request is not null ? new Permission
        {
            CircleManagment = request.CircleManagment,
            MemberManagement = request.MemberManagement,
            ProposalManagment = request.ProposalManagment,
            FeedManagment = request.FeedManagment,
            SessionManagment = request.SessionManagment,
            TodoTaskManagment = request.TodoTaskManagment
        } : null;

        await db.SaveChangesAsync(ct);
    }
}
