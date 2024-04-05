﻿using Core.ValueObjects;

using DataAccess.Base.Interfaces;

using TeamsMaker.Api.Contracts.Requests.Circle;
using TeamsMaker.Api.Contracts.Requests.JoinRequest;
using TeamsMaker.Api.Contracts.Responses.Circle;
using TeamsMaker.Api.Contracts.Responses.Profile;
using TeamsMaker.Api.Core.Consts;
using TeamsMaker.Api.Core.Enums;
using TeamsMaker.Api.DataAccess.Context;
using TeamsMaker.Api.Services.Circles.Interfaces;
using TeamsMaker.Api.Services.Files.Interfaces;
using TeamsMaker.Api.Services.JoinRequests.Interfaces;
using TeamsMaker.Core.Enums;

namespace TeamsMaker.Api.Services.Circles;

public class CircleService
    (AppDBContext db, IServiceProvider serviceProvider,
    IUserInfo userInfo, ICircleValidationService validationService,
    IJoinRequestService joinRequestService) : ICircleService, IPermissionService
{
    public async Task<Guid> AddAsync(AddCircleRequest request, CancellationToken ct)
    {
        if (userInfo.Roles.Contains(AppRoles.Student) &&
            await db.CircleMembers.AnyAsync(cm => cm.UserId == userInfo.UserId, ct))
            throw new ArgumentException("Student Cannot Be In Two Circles");

        using var transaction = await db.Database.BeginTransactionAsync(ct);

        var circle = new Circle
        {
            Name = request.Name,
            SummaryData = request.Summary,
            DefaultPermission = new Permission(),
            CircleMembers = [
                new CircleMember
                {
                    UserId = userInfo.UserId,
                    Badge = MemberBadges.Owner,
                    IsOwner = true
                }
            ],
        };

        await db.Circles.AddAsync(circle, ct);
        await db.SaveChangesAsync(ct);

        foreach (var studentId in request.InvitedStudents ?? [])
            await joinRequestService.AddAsync(new AddJoinRequest
            {
                CircleId = circle.Id,
                StudentId = studentId,
                SenderType = InvitationTypes.Circle
            }, ct);

        await transaction.CommitAsync(ct);

        return circle.Id;
    }

    public async Task<GetCircleResponse> GetAsync(Guid circleId, CancellationToken ct)
    {
        var circle = await db.Circles
            .Include(c => c.DefaultPermission)
            .Include(c => c.Skills)
            .Include(c => c.SummaryData)
            .Include(c => c.Links)
            .Include(c => c.CircleMembers)
            .SingleOrDefaultAsync(c => c.Id == circleId, ct) ??
            throw new ArgumentException("Invalid Circle ID");

        IFileService fileService = serviceProvider.GetRequiredKeyedService<IFileService>(BaseTypes.Circle);

        var response = new GetCircleResponse
        {
            Id = circle.Id,
            Name = circle.Name,
            Keywords = circle.Keywords != null ? circle.Keywords.Split(',') : [],
            IsPublic = circle.SummaryData?.IsPublic ?? false,
            Rate = circle.Rate,
            Status = circle.Status,
            OrganizationId = circle.OrganizationId,
            Skills = circle.Skills.Select(l => l.Name).ToList(),
            Links = circle.Links.Select(l => new LinkInfo { Type = l.Type, Url = l.Url }).ToList(),

            Avatar = fileService.GetFileUrl(circleId.ToString(), FileTypes.Avatar),
            Header = fileService.GetFileUrl(circleId.ToString(), FileTypes.Header)
        };


        if (circle.CircleMembers.Any(cm => cm.UserId == userInfo.UserId))
        {
            response.Summary = circle.SummaryData?.Summary;

            response.DefaultPermission = new PermissionsInfo
            {
                CircleManagment = circle.DefaultPermission.CircleManagment,
                MemberManagement = circle.DefaultPermission.MemberManagement,
                ProposalManagment = circle.DefaultPermission.ProposalManagment,
                FeedManagment = circle.DefaultPermission.FeedManagment,
                SessionManagment = circle.DefaultPermission.SessionManagment,
                TodoTaskManagment = circle.DefaultPermission.TodoTaskManagment
            };
        }

        return response;
    }

    public async Task<GetCircleMembersResponse> GetMembersAsync(Guid circleId, CancellationToken ct)
    {
        var circle = await db.Circles
            .Include(c => c.CircleMembers)
                .ThenInclude(cm => cm.ExceptionPermission)
            .Include(c => c.CircleMembers)
                .ThenInclude(cm => cm.User)
            .SingleOrDefaultAsync(c => c.Id == circleId, ct) ??
            throw new ArgumentException("Invalid Circle ID");

        var studentFileService = serviceProvider.GetRequiredKeyedService<IFileService>(BaseTypes.Student);
        var staffFileService = serviceProvider.GetRequiredKeyedService<IFileService>(BaseTypes.Staff);

        var response = new GetCircleMembersResponse
        {
            Members = await Task.WhenAll(circle.CircleMembers
            .Select(async cm =>
            {
                var avatarUrl = await db.Students.AnyAsync(s => s.Id == cm.UserId, ct)
                    ? studentFileService.GetFileUrl(cm.UserId, FileTypes.Avatar)
                    : staffFileService.GetFileUrl(cm.UserId, FileTypes.Avatar);

                return new CircleMemberInfo
                {
                    CircleMemberId = cm.Id,
                    UserId = cm.UserId,
                    FirstName = cm.User.FirstName,
                    LastName = cm.User.LastName,
                    Avatar = avatarUrl,
                    IsOwner = cm.IsOwner,
                    Badge = cm.Badge,
                    Bio = cm.User.Bio,
                    ExceptionPermissions = cm.ExceptionPermission == null ? null : new PermissionsInfo
                    {
                        CircleManagment = cm.ExceptionPermission.CircleManagment,
                        FeedManagment = cm.ExceptionPermission.FeedManagment,
                        MemberManagement = cm.ExceptionPermission.MemberManagement,
                        ProposalManagment = cm.ExceptionPermission.ProposalManagment,
                        SessionManagment = cm.ExceptionPermission.SessionManagment,
                        TodoTaskManagment = cm.ExceptionPermission.TodoTaskManagment
                    }
                };
            }).ToList())
        };

        return response;
    }

    public async Task UpvoteAsync(Guid circleId, CancellationToken ct)
    {
        if (!await db.Circles.AnyAsync(c => c.Id == circleId, ct))
            throw new ArgumentException();

        if (await db.Upvotes.CountAsync(upvote => upvote.CircleId == circleId && upvote.UserId == userInfo.UserId, ct) > 1)
            throw new InvalidOperationException();

        using var transaction = await db.Database.BeginTransactionAsync(ct);

        Upvote upvote = new()
        {
            UserId = userInfo.UserId,
            CircleId = circleId
        };

        await db.Upvotes.AddAsync(upvote, ct);
        await db.SaveChangesAsync(ct);

        await db
            .Circles
            .Where(c => c.Id == circleId)
            .ExecuteUpdateAsync(setters => setters.SetProperty(c => c.Rate, c => c.Rate + 1), ct);

        await transaction.CommitAsync(ct);
    }

    public async Task DownvoteAsync(Guid id, CancellationToken ct)
    {
        using var transaction = await db.Database.BeginTransactionAsync(ct);

        var upvote = await db.Upvotes.FindAsync([id], ct) ?? throw new NullReferenceException();

        if (await db.Upvotes.CountAsync(upvote => upvote.CircleId == upvote.CircleId && upvote.UserId == userInfo.UserId, ct) == 0)
            throw new InvalidOperationException();

        var circleId = upvote.CircleId;

        db.Upvotes.Remove(upvote);

        await db.SaveChangesAsync(ct);

        await db
            .Circles
            .Where(c => c.Id == circleId)
            .ExecuteUpdateAsync(setters => setters.SetProperty(c => c.Rate, c => c.Rate - 1), cancellationToken: ct);

        await transaction.CommitAsync(ct);
    }

    // CircleManagment
    public async Task UpdateInfoAsync(Guid circleId, UpdateCircleInfoRequest request, CancellationToken ct)
    {
        var circleMember = await validationService.TryGetCircleMemberAsync(userInfo.UserId, circleId, ct);

        var circle = await db.Circles
            .Include(c => c.DefaultPermission)
            .Include(c => c.SummaryData)
            .Include(c => c.Skills)
            .Include(c => c.Links)
            .SingleOrDefaultAsync(c => c.Id == circleId, ct) ??
            throw new ArgumentException("Invalid Circle ID");

        validationService.CheckPermission(circleMember, circle, PermissionsEnum.CircleManagement);

        circle.Name = request.Name;
        circle.Keywords = request.Keywords != null ? string.Join(",", request.Keywords) : null;

        db.Skills.RemoveRange(circle.Skills);
        circle.Skills = request.Skills?.Select(s => new Skill { CircleId = circleId, Name = s }).ToList() ?? [];

        db.Links.RemoveRange(circle.Links);
        circle.Links = request.Links?.Select(l => new Link { CircleId = circleId, Url = l.Url, Type = l.Type }).ToList() ?? [];

        if (request.Summary != null)
        {
            bool isPublic = false;
            if (circle.SummaryData != null)
                isPublic = circle.SummaryData.IsPublic;

            circle.SummaryData = new SummaryData { Summary = request.Summary, IsPublic = isPublic };
        }
        else
            circle.SummaryData = null;

        await db.SaveChangesAsync(ct);
    }

    public async Task UpdatePrivacyAsync(Guid circleId, bool isPublic, CancellationToken ct)
    {
        var circleMember = await validationService.TryGetCircleMemberAsync(userInfo.UserId, circleId, ct);

        var circle = await db.Circles
            .Include(c => c.DefaultPermission)
            .Include(c => c.SummaryData)
            .SingleOrDefaultAsync(c => c.Id == circleId, ct) ??
            throw new ArgumentException("Invalid Circle ID");

        validationService.CheckPermission(circleMember, circle, PermissionsEnum.CircleManagement);

        if (circle.SummaryData != null)
            circle.SummaryData.IsPublic = isPublic;

        await db.SaveChangesAsync(ct);
    }

    // DangerZone
    public async Task UpdatePermissionAsync(Guid circleId, UpdatePermissionRequest? request, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(request);

        await validationService.TryGetOwnerAsync(userInfo.UserId, circleId, ct);

        var circle = await db.Circles
            .Include(c => c.DefaultPermission)
            .SingleOrDefaultAsync(c => c.Id == circleId, ct) ??
            throw new ArgumentException("Invalid Circle Id");

        db.Permissions.Remove(circle.DefaultPermission);
        circle.DefaultPermission = new Permission
        {
            CircleManagment = request.CircleManagment,
            MemberManagement = request.MemberManagement,
            ProposalManagment = request.ProposalManagment,
            FeedManagment = request.FeedManagment,
            SessionManagment = request.SessionManagment,
            TodoTaskManagment = request.TodoTaskManagment
        };

        await db.SaveChangesAsync(ct);
    }

    public async Task TransferOwnershipAsync(Guid circleId, string newOwnerId, CancellationToken ct)
    {
        var oldOwner = await validationService.TryGetOwnerAsync(userInfo.UserId, circleId, ct);

        var newOwner = await validationService.TryGetCircleMemberAsync(newOwnerId, circleId, ct);

        oldOwner.IsOwner = false;
        newOwner.IsOwner = true;

        if (newOwner.ExceptionPermission is not null)
            db.Permissions.Remove(newOwner.ExceptionPermission);

        await db.SaveChangesAsync(ct);
    }

    public async Task ArchiveAsync(Guid circleId, CancellationToken ct)
    {
        var owner = await validationService.TryGetOwnerAsync(userInfo.UserId, circleId, ct);

        var circle = await db.Circles.FindAsync([circleId], ct) ??
            throw new ArgumentException("Invalid Circle Id");

        circle.Status = CircleStatusEnum.Archived;

        await db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid circleId, CancellationToken ct)
    {
        var owner = await validationService.TryGetOwnerAsync(userInfo.UserId, circleId, ct);

        var circle = await db.Circles
            .Include(c => c.CircleMembers)
                .ThenInclude(cm => cm.ExceptionPermission)
            .Include(c => c.DefaultPermission)
            .Include(c => c.Links)
            .Include(c => c.Skills)
            .Include(c => c.Invitions)
            .Include(c => c.Upvotes)
            .Include(c => c.TodoTasks)
            .Include(c => c.Sessions)
            .SingleOrDefaultAsync(c => c.Id == circleId, ct) ??
            throw new ArgumentException("Invalid Circle Id");

        // Todo: if Circle has an accepted , throw exception cuz it can't be deleted

        db.Permissions.RemoveRange(
            circle.CircleMembers
                .Select(cm => cm.ExceptionPermission)
                .Where(p => p != null)!);

        db.Permissions.Remove(circle.DefaultPermission);

        db.CircleMembers.RemoveRange(circle.CircleMembers);

        db.Links.RemoveRange(circle.Links);

        db.Skills.RemoveRange(circle.Skills);

        db.JoinRequests.RemoveRange(circle.Invitions);

        db.Upvotes.RemoveRange(circle.Upvotes);

        db.TodoTasks.RemoveRange(circle.TodoTasks);

        db.Sessions.RemoveRange(circle.Sessions);

        db.Circles.Remove(circle);

        await db.SaveChangesAsync(ct);
    }
}