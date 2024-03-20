using Core.ValueObjects;

using DataAccess.Base.Interfaces;

using TeamsMaker.Api.Contracts.Requests.Circle;
using TeamsMaker.Api.Contracts.Responses.Circle;
using TeamsMaker.Api.Contracts.Responses.Profile;
using TeamsMaker.Api.Core.Consts;
using TeamsMaker.Api.Core.Enums;
using TeamsMaker.Api.DataAccess.Context;
using TeamsMaker.Api.Services.Circles.Interfaces;
using TeamsMaker.Api.Services.Files.Interfaces;
using TeamsMaker.Core.Enums;

namespace TeamsMaker.Api.Services.Circles;

public class CircleService
    (AppDBContext db, IServiceProvider serviceProvider, IUserInfo userInfo, ICirclePermissionService permissionService) : ICircleService
{
    private readonly IFileService _fileService = serviceProvider.GetRequiredKeyedService<IFileService>(BaseTypes.Circle);

    public async Task AddAsync(AddCircleRequest request, CancellationToken ct)
    {
        if (userInfo.Roles.Contains(AppRoles.Student) &&
            await db.CircleMembers.AnyAsync(cm => cm.UserId == userInfo.UserId, ct))
            throw new ArgumentException("Student Cannot Be In Two Circles");

        using var transaction = await db.Database.BeginTransactionAsync(ct);
        var circle = new Circle
        {
            Name = request.Name,
            Description = request.Description,
            Summary = request.Summary,
        };
        await db.Circles.AddAsync(circle, ct);

        circle.Skills = request.Skills?.Select(s => new Skill { CircleId = circle.Id, Name = s }).ToList() ?? [];
        circle.Links = request.Links?.Select(l => new Link { CircleId = circle.Id, Url = l.Url, Type = l.Type }).ToList() ?? [];

        var circleMember = new CircleMember
        {
            UserId = userInfo.UserId,
            CircleId = circle.Id,
            IsOwner = true,
            ExceptionPermission = new Permission
            {
                CircleManagment = true,
                FeedManagment = true,
                MemberManagement = true,
                ProposalManagment = true
            }
        };

        await db.CircleMembers.AddAsync(circleMember, ct);

        await db.SaveChangesAsync(ct);
        await transaction.CommitAsync(ct);
    }

    public async Task<GetCircleResponse> GetAsync(Guid circleId, CancellationToken ct)
    {
        var circle = await db.Circles
            .Include(c => c.DefaultPermission)
            .Include(c => c.Skills)
            .Include(c => c.Summary)
            .Include(c => c.Links)
            .Include(c => c.CircleMembers)
            .SingleOrDefaultAsync(c => c.Id == circleId, ct) ??
            throw new ArgumentException("Invalid Circle ID");

        var response = new GetCircleResponse
        {
            Name = circle.Name,
            Description = circle.Description,
            IsPublic = circle.Summary?.IsPublic ?? false,
            Rate = circle.Rate,
            Status = circle.Status,
            OrganizationId = circle.OrganizationId,
            Skills = circle.Skills.Select(l => l.Name).ToList(),
            Links = circle.Links.Select(l => new LinkInfo { Type = l.Type, Url = l.Url }).ToList(),

            Avatar = _fileService.GetFileUrl(circleId.ToString(), FileTypes.Avatar),
            Header = _fileService.GetFileUrl(circleId.ToString(), FileTypes.Header)
        };


        if (circle.CircleMembers.Any(cm => cm.UserId == userInfo.UserId))
        {
            response.Summary = circle.Summary?.Summary;

            response.DefaultPermission = new PermissionsInfo
            {
                CircleManagment = circle.DefaultPermission.CircleManagment,
                MemberManagement = circle.DefaultPermission.MemberManagement,
                ProposalManagment = circle.DefaultPermission.ProposalManagment,
                FeedManagment = circle.DefaultPermission.FeedManagment
            };
        }

        return response;
    }

    public async Task<GetCircleMembersResponse> GetMembersAsync(Guid circleId, CancellationToken ct)
    {
        var circle = await db.Circles
            .Include(c => c.CircleMembers)
                .ThenInclude(cm => cm.ExceptionPermission)
            .SingleOrDefaultAsync(c => c.Id == circleId, ct) ??
            throw new ArgumentException("Invalid Circle ID");

        var response = new GetCircleMembersResponse
        {
            Members = circle.CircleMembers
                .Select(cm => new CircleMemberInfo
                {
                    UserId = cm.UserId,
                    IsOwner = cm.IsOwner,
                    Badge = cm.Badge,
                    Permissions = cm.ExceptionPermission == null ? null : new PermissionsInfo
                    {
                        CircleManagment = cm.ExceptionPermission.CircleManagment,
                        FeedManagment = cm.ExceptionPermission.FeedManagment,
                        MemberManagement = cm.ExceptionPermission.MemberManagement,
                        ProposalManagment = cm.ExceptionPermission.ProposalManagment
                    }
                }).ToList()
        };

        return response;
    }

    // CircleManagment
    public async Task UpdateInfoAsync(Guid circleId, UpdateCircleInfoRequest request, CancellationToken ct)
    {
        var circleMember = await permissionService.TryGetCircleMemberAsync(userInfo.UserId, circleId, ct);

        var circle = await db.Circles
            .Include(c => c.DefaultPermission)
            .Include(c => c.Summary)
            .Include(c => c.Skills)
            .SingleOrDefaultAsync(c => c.Id == circleId, ct) ??
            throw new ArgumentException("Invalid Circle ID");

        permissionService.HasPermission(circleMember, circle, PermissionsEnum.CircleManagement);

        circle.Name = request.Name;
        circle.Description = request.Description;

        db.Skills.RemoveRange(circle.Skills);
        circle.Skills = request.Skills?.Select(s => new Skill { CircleId = circleId, Name = s }).ToList() ?? [];

        if (request.Summary != null)
        {
            bool isPublic = false;
            if (circle.Summary != null)
                isPublic = circle.Summary.IsPublic;

            circle.Summary = new SummaryData { Summary = request.Summary, IsPublic = isPublic };
        }
        else
            circle.Summary = null;

        await db.SaveChangesAsync(ct);
    }

    public async Task UpdateLinksAsync(Guid circleId, UpdateCircleLinksRequest request, CancellationToken ct)
    {
        var circleMember = await permissionService.TryGetCircleMemberAsync(userInfo.UserId, circleId, ct);

        var circle = await db.Circles
            .Include(c => c.DefaultPermission)
            .Include(c => c.Links)
            .SingleOrDefaultAsync(c => c.Id == circleId, ct) ??
            throw new ArgumentException("Invalid Circle ID");

        permissionService.HasPermission(circleMember, circle, PermissionsEnum.CircleManagement);

        db.Links.RemoveRange(circle.Links);
        circle.Links = request.Links?.Select(l => new Link { CircleId = circleId, Url = l.Url, Type = l.Type }).ToList() ?? [];

        await db.SaveChangesAsync(ct);
    }

    public async Task UpdatePrivacyAsync(Guid circleId, bool isPublic, CancellationToken ct)
    {
        var circleMember = await permissionService.TryGetCircleMemberAsync(userInfo.UserId, circleId, ct);

        var circle = await db.Circles
            .Include(c => c.DefaultPermission)
            .Include(c => c.Summary)
            .SingleOrDefaultAsync(c => c.Id == circleId, ct) ??
            throw new ArgumentException("Invalid Circle ID");

        permissionService.HasPermission(circleMember, circle, PermissionsEnum.CircleManagement);

        if (circle.Summary != null)
            circle.Summary.IsPublic = isPublic;

        await db.SaveChangesAsync(ct);
    }

    // DangerZone
    public async Task UpdateDefaultPermissionAsync(Guid circleId, UpdateDeafultPermissionRequest request, CancellationToken ct)
    {
        var owner = await permissionService.TryGetOwnerAsync(userInfo.UserId, circleId, ct);

        var circle = await db.Circles
            .Include(c => c.DefaultPermission)
            .SingleOrDefaultAsync(c => c.Id == circleId, ct) ??
            throw new ArgumentException("Invalid Circle Id");

        circle.DefaultPermission = new Permission
        {
            CircleManagment = request.CircleManagment,
            MemberManagement = request.MemberManagement,
            ProposalManagment = request.ProposalManagment,
            FeedManagment = request.FeedManagment
        };

        await db.SaveChangesAsync(ct);
    }

    public async Task ChangeOwnershipAsync(Guid circleId, string newOwnerId, CancellationToken ct)
    {
        var oldOwner = await permissionService.TryGetOwnerAsync(userInfo.UserId, circleId, ct);

        var newOwner = await permissionService.TryGetCircleMemberAsync(newOwnerId, circleId, ct);

        oldOwner.IsOwner = false;
        newOwner.IsOwner = true;

        await db.SaveChangesAsync(ct);
    }

    public async Task ArchiveAsync(Guid circleId, CancellationToken ct)
    {
        var owner = await permissionService.TryGetOwnerAsync(userInfo.UserId, circleId, ct);

        var circle = await db.Circles.FindAsync([circleId], ct) ??
            throw new ArgumentException("Invalid Circle Id");

        circle.Status = CircleStatusEnum.Archived;

        await db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid circleId, CancellationToken ct)
    {
        var owner = await permissionService.TryGetOwnerAsync(userInfo.UserId, circleId, ct);

        var circle = await db.Circles
            .Include(c => c.CircleMembers)
                .ThenInclude(cm => cm.ExceptionPermission)
            .Include(c => c.Links)
            .Include(c => c.Skills)
            .Include(c => c.Invitions)
            .Include(c => c.TodoTasks)
            .Include(c => c.Sessions)
            .SingleOrDefaultAsync(c => c.Id == circleId, ct) ??
            throw new ArgumentException("Invalid Circle Id");

        // Todo: if Circle has an accepted , throw exception cuz it can't be deleted

        db.Permissions.RemoveRange(
            circle.CircleMembers
                .Select(cm => cm.ExceptionPermission)
                .Where(p => p != null)!);

        db.CircleMembers.RemoveRange(circle.CircleMembers);

        db.Links.RemoveRange(circle.Links);

        db.Skills.RemoveRange(circle.Skills);

        db.JoinRequests.RemoveRange(circle.Invitions);

        db.TodoTasks.RemoveRange(circle.TodoTasks);

        db.Sessions.RemoveRange(circle.Sessions);

        db.Circles.Remove(circle);

        await db.SaveChangesAsync(ct);
    }
}

// api/circle/accept
// api/joinRequest/accept/{id} = true
// //TODO: filterdBy, 