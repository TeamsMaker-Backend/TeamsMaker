using DataAccess.Base.Interfaces;

using TeamsMaker.Api.Contracts.Requests.Circle;
using TeamsMaker.Api.Core.Consts;
using TeamsMaker.Api.Core.Enums;
using TeamsMaker.Api.DataAccess.Context;
using TeamsMaker.Api.Services.Circles.Interfaces;

namespace TeamsMaker.Api.Services.Circles;

public class CircleMemberService
    (AppDBContext db, IUserInfo userInfo, ICircleValidationService validationService, IServiceProvider serviceProvider) : ICircleMemberService, IPermissionService
{
    public async Task AddAsync(Guid circleId, string userId, string reciever, CancellationToken ct)
    {
        if (!await db.Students.AnyAsync(s => s.Id == userId, ct))
            throw new ArgumentException("Invalid Student Id");

        var circle = await db.Circles
            .Include(c => c.DefaultPermission)
            .Include(c => c.CircleMembers)
            .SingleOrDefaultAsync(c => c.Id == circleId, ct) ??
            throw new ArgumentException("Invalid Circle Id");

        if (reciever == InvitationTypes.Circle)
        {
            var circleMember = await validationService.TryGetCircleMemberAsync(userInfo.UserId, circleId, ct);

            validationService.CheckPermission(circleMember, circle, PermissionsEnum.MemberManagement);
        }

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
            .Include(c => c.CircleMembers)
            .SingleOrDefaultAsync(c => c.Id == removedCircleMember.CircleId, ct) ??
            throw new ArgumentException("Invalid Circle ID");

        bool isCircleDeleted = false;

        if (userInfo.UserId != removedCircleMember.UserId)
            validationService.CheckPermission(currentCircleMember, circle, PermissionsEnum.MemberManagement);
        else if (currentCircleMember.IsOwner) // leaves (removes himself)
        {
            if (circle.CircleMembers.Count > 1)
                throw new ArgumentException("Transfer Ownership First");
            else
            {
                var circleService = serviceProvider.GetRequiredService<ICircleService>();
                await circleService.DeleteAsync(circle.Id, ct);
                isCircleDeleted = true;
            }
        }

        if (!isCircleDeleted)
            db.CircleMembers.Remove(removedCircleMember);

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
            CircleManagment = request.CircleManagment ?? default,
            MemberManagement = request.MemberManagement ?? default,
            ProposalManagment = request.ProposalManagment ?? default,
            FeedManagment = request.FeedManagment ?? default,
            SessionManagment = request.SessionManagment ?? default,
            TodoTaskManagment = request.TodoTaskManagment ?? default
        } : null;

        await db.SaveChangesAsync(ct);
    }
}
