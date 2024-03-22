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

        var circle = await db.Circles
            .Include(c => c.CircleMembers)
            .SingleOrDefaultAsync(c => c.Id == circleId, ct) ??
            throw new ArgumentException("Invalid Circle Id");

        if (await db.CircleMembers.AnyAsync(cm => cm.UserId == userId, ct))
            throw new ArgumentException("Student Cannot Be In Two Circles");

        circle.CircleMembers.Add(new CircleMember
        {
            UserId = userId,
            IsOwner = false,
        });

        await db.SaveChangesAsync(ct);
    }

    public async Task RemoveAsync(Guid circleMemberId, CancellationToken ct)
    {
        var circleMember = await db.CircleMembers
            .FindAsync([circleMemberId], ct) ??
            throw new ArgumentException("Not a circle Member");

        var circle = await db.Circles
            .Include(c => c.DefaultPermission)
            .SingleOrDefaultAsync(c => c.Id == circleMember.CircleId, ct) ??
            throw new ArgumentException("Invalid Circle ID");

        // Hasnot Permission to remove a circle member
        if (userInfo.UserId != circleMember.UserId &&
        !validationService.HasPermission(circleMember, circle, PermissionsEnum.MemberManagement))
            throw new ArgumentException("Donot Have The Permission!");

        db.CircleMembers.Remove(circleMember);

        await db.SaveChangesAsync(ct);
    }

    public async Task UpdateBadgeAsync(Guid circleMemberId, string? badge, CancellationToken ct)
    {
        var circleMember = await db.CircleMembers
            .FindAsync([circleMemberId], ct) ??
            throw new ArgumentException("Not a circle Member");

        var circle = await db.Circles
            .Include(c => c.DefaultPermission)
            .SingleOrDefaultAsync(c => c.Id == circleMember.CircleId, ct) ??
            throw new ArgumentException("Invalid Circle ID");

        // Hasnot Permission to update a circle member
        if (userInfo.UserId != circleMember.UserId &&
        !validationService.HasPermission(circleMember, circle, PermissionsEnum.MemberManagement))
            throw new ArgumentException("Donot Have The Permission!");

        circleMember.Badge = badge; // Todo: string splitting ','

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
            FeedManagment = request.FeedManagment
        } : null;

        await db.SaveChangesAsync(ct);
    }
}
