using Core.ValueObjects;

using DataAccess.Base.Interfaces;

using TeamsMaker.Api.Contracts.Requests.Circle;
using TeamsMaker.Api.Contracts.Requests.JoinRequest;
using TeamsMaker.Api.Contracts.Responses.Circle;
using TeamsMaker.Api.Contracts.Responses.JoinRequest;
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
        {
            // check whether student id is valid
            if (await db.Students.AnyAsync(s => s.Id == studentId, ct) == false)
                throw new ArgumentException("Invalid Student ID!");

            await joinRequestService.AddAsync(new AddJoinRequest
            {
                CircleId = circle.Id,
                StudentId = studentId,
                SenderType = InvitationTypes.Circle
            }, ct);
        }

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
                .ThenInclude(cm => cm.User)
            .Include(c => c.Upvotes)
            .Include(c => c.Invitions)
                .ThenInclude(i => i.Student)
            .SingleOrDefaultAsync(c => c.Id == circleId, ct) ??
            throw new ArgumentException("Invalid Circle ID");

        var studentFileService = serviceProvider.GetRequiredKeyedService<IFileService>(BaseTypes.Student);
        var circleFileService = serviceProvider.GetRequiredKeyedService<IFileService>(BaseTypes.Circle);

        var response = new GetCircleResponse
        {
            Id = circle.Id,
            Name = circle.Name,
            Keywords = circle.Keywords != null ? circle.Keywords.Split(',') : [],
            IsPublic = circle.SummaryData?.IsPublic ?? false,
            Rate = circle.Rate,

            OwnerName = circle.CircleMembers
                .Where(cm => cm.IsOwner)
                .Select(cm => $"{cm.User.FirstName} {cm.User.LastName}")
                .Single(),
            IsUpvoted = circle.Upvotes.Any(up => up.UserId == userInfo.UserId),
            Status = circle.Status,
            OrganizationId = circle.OrganizationId,
            Skills = circle.Skills.Select(l => l.Name).ToList(),
            Links = circle.Links.Select(l => new LinkInfo { Type = l.Type, Url = l.Url }).ToList(),

            Avatar = circleFileService.GetFileUrl(circleId.ToString(), FileTypes.Avatar),
            Header = circleFileService.GetFileUrl(circleId.ToString(), FileTypes.Header)
        };


        if (circle.CircleMembers.Any(cm => cm.UserId == userInfo.UserId))
        {
            response.CircleJoinRequests = new()
            {
                JoinRequests = circle.Invitions
                    .Where(jr => jr.Sender == InvitationTypes.Student) // join request from users
                    .Where(jr => jr.IsAccepted == false)
                    .OrderByDescending(jr => jr.CreationDate)
                    .Take(3)
                    .Select(jr => new GetBaseJoinRequestResponse
                    {
                        JoinRequestId = jr.Id,
                        Sender = jr.Sender,
                        OtherSideName = jr.Student.FirstName + " " + jr.Student.LastName,
                        Avatar = studentFileService.GetFileUrl(jr.StudentId, FileTypes.Avatar)
                    })
                    .ToList(),

                Invitations = circle.Invitions
                    .Where(jr => jr.Sender == InvitationTypes.Circle) // invitation from circle
                    .Where(jr => jr.IsAccepted == false)
                    .OrderByDescending(jr => jr.CreationDate)
                    .Take(3)
                    .Select(jr => new GetBaseJoinRequestResponse
                    {
                        JoinRequestId = jr.Id,
                        Sender = jr.Sender,
                        OtherSideName = jr.Student.FirstName + " " + jr.Student.LastName,
                        Avatar = studentFileService.GetFileUrl(jr.StudentId, FileTypes.Avatar)
                    })
                    .ToList(),
            };

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

    public async Task<List<GetCircleAsRowResponse>> GetAsync(CancellationToken ct)
    {
        var fileService = serviceProvider.GetRequiredKeyedService<IFileService>(BaseTypes.Circle);

        // get user
        var circles = await db.CircleMembers
            .Include(cm => cm.Circle)
            .Where(cm => cm.UserId == userInfo.UserId)
            .Where(cm => validationService.HasPermission(cm, cm.Circle, PermissionsEnum.MemberManagement))
            .Select(cm => new GetCircleAsRowResponse
            {
                Id = cm.CircleId,
                Name = cm.Circle.Name,
                OwnerName = db.CircleMembers
                    .Include(cm => cm.User)
                    .Where(m => m.IsOwner && m.CircleId == cm.CircleId)
                    .Select(m => $"{m.User.FirstName} {m.User.LastName}")
                    .First(),
                Avatar = fileService.GetFileUrl(cm.CircleId.ToString(), FileTypes.Avatar)
            })
            .ToListAsync(ct);

        return circles;
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
            Members = circle.CircleMembers
                .Select(cm => new CircleMemberInfo
                {
                    CircleMemberId = cm.Id,
                    UserId = cm.UserId,
                    UserType = db.Students.Any(x => x.Id == cm.UserId) ? UserEnum.Student : UserEnum.Staff,
                    FirstName = cm.User.FirstName,
                    LastName = cm.User.LastName,
                    IsOwner = cm.IsOwner,
                    Badge = cm.Badge,
                    Role = cm.Role,
                    ExceptionPermissions = cm.ExceptionPermission == null ? null : new PermissionsInfo
                    {
                        CircleManagment = cm.ExceptionPermission.CircleManagment,
                        FeedManagment = cm.ExceptionPermission.FeedManagment,
                        MemberManagement = cm.ExceptionPermission.MemberManagement,
                        ProposalManagment = cm.ExceptionPermission.ProposalManagment,
                        SessionManagment = cm.ExceptionPermission.SessionManagment,
                        TodoTaskManagment = cm.ExceptionPermission.TodoTaskManagment
                    }
                })
                .ToList()
        };

        foreach (var info in response.Members)
        {
            info.Avatar = await db.Students.AnyAsync(s => s.Id == info.UserId, ct)
                    ? studentFileService.GetFileUrl(info.UserId, FileTypes.Avatar)
                    : staffFileService.GetFileUrl(info.UserId, FileTypes.Avatar);
        }

        return response;
    }

    public async Task UpvoteAsync(Guid circleId, CancellationToken ct)
    {
        if (!await db.Circles.AnyAsync(c => c.Id == circleId, ct))
            throw new ArgumentException("Invalid Circle ID!");

        if (await db.Upvotes.CountAsync(upvote => upvote.CircleId == circleId && upvote.UserId == userInfo.UserId, ct) >= 1)
            throw new ArgumentException("Already upvoted");

        using var transaction = await db.Database.BeginTransactionAsync(ct);

        Upvote upvote = new()
        {
            UserId = userInfo.UserId,
            CircleId = circleId
        };

        await db.Upvotes.AddAsync(upvote, ct);

        await db
            .Circles
            .Where(c => c.Id == circleId)
            .ExecuteUpdateAsync(setters => setters.SetProperty(c => c.Rate, c => c.Rate + 1), ct);

        await db.SaveChangesAsync(ct);
        await transaction.CommitAsync(ct);
    }

    public async Task DownvoteAsync(Guid id, CancellationToken ct)
    {
        var circle = await db.Circles
            .Include(c => c.Upvotes)
            .SingleOrDefaultAsync(c => c.Id == id, ct)
            ?? throw new NullReferenceException("This circle not found");

        var upvote = circle.Upvotes
            .SingleOrDefault(upvote => upvote.UserId == userInfo.UserId)
            ?? throw new InvalidOperationException("This circle not upvoted");

        circle.Rate--;
        db.Upvotes.Remove(upvote);

        await db.SaveChangesAsync(ct);
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

        if (!string.IsNullOrEmpty(request.Name)) circle.Name = request.Name;

        if (request.Summary != null)
        {
            bool isPublic = false;
            if (circle.SummaryData != null)
                isPublic = circle.SummaryData.IsPublic;

            circle.SummaryData = new SummaryData { Summary = request.Summary, IsPublic = isPublic };
        }

        // and check length
        circle.Keywords = request.Keywords != null && request.Keywords.Any() ? string.Join(",", request.Keywords) : null;

        db.Skills.RemoveRange(circle.Skills);
        circle.Skills = request.Skills?.Select(s => new Skill { CircleId = circleId, Name = s }).ToList() ?? [];

        db.Links.RemoveRange(circle.Links);
        circle.Links = request.Links?.Select(l => new Link { CircleId = circleId, Url = l.Url, Type = l.Type }).ToList() ?? [];


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

        if (request.MemberManagement.HasValue) circle.DefaultPermission.MemberManagement = request.MemberManagement.Value;
        if (request.CircleManagment.HasValue) circle.DefaultPermission.CircleManagment = request.CircleManagment.Value;
        if (request.FeedManagment.HasValue) circle.DefaultPermission.FeedManagment = request.FeedManagment.Value;
        if (request.ProposalManagment.HasValue) circle.DefaultPermission.ProposalManagment = request.ProposalManagment.Value;
        if (request.SessionManagment.HasValue) circle.DefaultPermission.SessionManagment = request.SessionManagment.Value;
        if (request.TodoTaskManagment.HasValue) circle.DefaultPermission.TodoTaskManagment = request.TodoTaskManagment.Value;

        await db.SaveChangesAsync(ct);
    }

    public async Task TransferOwnershipAsync(Guid circleId, string newOwnerId, CancellationToken ct)
    {
        var oldOwner = await validationService.TryGetOwnerAsync(userInfo.UserId, circleId, ct);

        var newOwner = await validationService.TryGetCircleMemberAsync(newOwnerId, circleId, ct);

        oldOwner.IsOwner = false;

        newOwner.IsOwner = true;
        newOwner.Badge = MemberBadges.Owner;

        if (newOwner.ExceptionPermission is not null)
            db.Permissions.Remove(newOwner.ExceptionPermission);

        await db.SaveChangesAsync(ct);
    }

    public async Task ArchiveAsync(Guid circleId, CancellationToken ct)
    {
        var circle = await db.Circles.FindAsync([circleId], ct) ??
            throw new ArgumentException("Invalid Circle Id");

        await validationService.TryGetOwnerAsync(userInfo.UserId, circleId, ct);

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