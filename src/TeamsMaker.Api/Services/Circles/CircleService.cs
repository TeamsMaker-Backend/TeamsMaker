using Core.Generics;
using Core.ValueObjects;

using DataAccess.Base.Interfaces;

using Microsoft.IdentityModel.Tokens;

using TeamsMaker.Api.Contracts.QueryStringParameters;

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
        var activeCircles = await db.CircleMembers
            .CountAsync(cm => cm.UserId == userInfo.UserId && cm.Circle.Status == CircleStatusEnum.Active, ct);

        if (userInfo.Roles.Contains(AppRoles.Student) && activeCircles > 1)
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

        var circleMembersQuery = await db.CircleMembers
            .Include(cm => cm.Circle)
            .Where(cm => cm.UserId == userInfo.UserId)
            .Where(cm => cm.Circle.Status == CircleStatusEnum.Active)
            .ToListAsync(ct);

        var circles = circleMembersQuery
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
            .ToList();

        return circles;
    }

    public async Task<PagedList<GetCircleAsRowResponse>> GetAsync(BaseQueryStringWithQ query, CancellationToken ct)
    {
        var fileService = serviceProvider.GetRequiredKeyedService<IFileService>(BaseTypes.Circle);

        var circlesQuery = db.Circles
            .Include(c => c.CircleMembers)
                .ThenInclude(cm => cm.User)
            .Where(c => c.Status == CircleStatusEnum.Active)
            .AsQueryable();

        if (!string.IsNullOrEmpty(query.Q))
            circlesQuery = circlesQuery
                .Where(c => c.Name.Contains(query.Q)
                        || c.CircleMembers.Any(cm => cm.User.FirstName.Contains(query.Q)
                                                || cm.User.LastName.Contains(query.Q)
                                                || cm.User.Email!.Contains(query.Q)));

        circlesQuery = circlesQuery.OrderByDescending(c => c.CreationDate);

        if (!string.IsNullOrEmpty(query.Q))
            circlesQuery = circlesQuery.OrderBy(c => c.Name.Contains(query.Q));


        var circles = circlesQuery
            .Select(c => new GetCircleAsRowResponse
            {
                Id = c.Id,
                Name = c.Name,
                OwnerName = c.CircleMembers
                    .Where(m => m.IsOwner && m.CircleId == c.Id)
                    .Select(m => $"{m.User.FirstName} {m.User.LastName}")
                    .First(),
                Avatar = fileService.GetFileUrl(c.Id.ToString(), FileTypes.Avatar)
            });

        return await PagedList<GetCircleAsRowResponse>.ToPagedListAsync(circles, query.PageNumber, query.PageSize, ct);
    }

    public async Task<PagedList<GetCircleAsCardResponse>> ListArchivedAsync(ArchiveQueryString archiveQuery, CancellationToken ct)
    {
        var fileService = serviceProvider.GetRequiredKeyedService<IFileService>(BaseTypes.Circle);

        var archivedCirclesQuery = db.Circles
            .Include(c => c.CircleMembers)
                .ThenInclude(cm => cm.User)
            .Include(c => c.Upvotes)
            .Where(c => c.Status == CircleStatusEnum.Archived);

        #region filtering

        if (!string.IsNullOrEmpty(archiveQuery.Q))
            archivedCirclesQuery = archivedCirclesQuery
                .Where(c => c.Name.Contains(archiveQuery.Q)
                        || c.CircleMembers.Any(cm => cm.User.FirstName.Contains(archiveQuery.Q)
                                                || cm.User.LastName.Contains(archiveQuery.Q)
                                                || cm.User.Email!.Contains(archiveQuery.Q)));


        if (archiveQuery.CreatedOn.HasValue)
            archivedCirclesQuery = archivedCirclesQuery.Where(c => c.CreationDate == archiveQuery.CreatedOn);


        if (archiveQuery.ArchivedOn.HasValue)
            archivedCirclesQuery = archivedCirclesQuery.Where(c => c.ArchivedOn == archiveQuery.ArchivedOn);


        if (!string.IsNullOrEmpty(archiveQuery.SupervisorId))
            archivedCirclesQuery = archivedCirclesQuery
                .Where(c => c.CircleMembers.Any(cm => cm.UserId == archiveQuery.SupervisorId));


        if (archiveQuery.DepartmentId.HasValue)
        {
            var staff = await db.Departments
                .Include(d => d.DepartmentStaff)
                .Where(d => d.Id == archiveQuery.DepartmentId.Value)
                .SelectMany(d => d.DepartmentStaff)
                .Select(st => st.StaffId)
                .ToListAsync(cancellationToken: ct);

            archivedCirclesQuery = archivedCirclesQuery
                .Where(c => c.CircleMembers.Any(cm => staff.Contains(cm.UserId)));
        }


        if (!string.IsNullOrEmpty(archiveQuery.Technologies))
            archivedCirclesQuery = archivedCirclesQuery
                .Where(c => string.IsNullOrEmpty(c.Keywords) || c.Keywords.Contains(archiveQuery.Technologies));
        #endregion


        #region ordering

        if (archiveQuery.SortByUpvotesDesc.HasValue && !archiveQuery.SortByUpvotesAsc.HasValue)
            archivedCirclesQuery = archivedCirclesQuery.OrderByDescending(c => c.Upvotes.Count());

        if (archiveQuery.SortByUpvotesAsc.HasValue && !archiveQuery.SortByUpvotesDesc.HasValue)
            archivedCirclesQuery = archivedCirclesQuery.OrderBy(c => c.Upvotes.Count());


        if (archiveQuery.SortByCreationDateAsc.HasValue)
            archivedCirclesQuery = archivedCirclesQuery.OrderBy(c => c.CreationDate);
        else
            archivedCirclesQuery = archivedCirclesQuery.OrderByDescending(c => c.CreationDate);
        #endregion


        var circlesCardsQuery = archivedCirclesQuery
            .Select(card => new GetCircleAsCardResponse
            {
                Id = card.Id,
                Avatar = fileService.GetFileUrl(card.Id.ToString(), FileTypes.Avatar),
                Keywords = string.IsNullOrEmpty(card.Keywords) ? null : card.Keywords,
                Github = card.Links.Any(l => l.Type == LinkTypesEnum.GitHub) ? card.Links.First(l => l.Type == LinkTypesEnum.GitHub).Url : null,
                Name = card.Name,
                OwnerName = $"{card.CircleMembers.First(cm => cm.IsOwner).User.FirstName} {card.CircleMembers.First(cm => cm.IsOwner).User.LastName}",
                Rate = card.Rate,
                Summary = card.SummaryData != null ? card.SummaryData.Summary : null
            });


        return await PagedList<GetCircleAsCardResponse>.ToPagedListAsync(circlesCardsQuery, archiveQuery.PageNumber, archiveQuery.PageSize, ct);
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
            .Include(c => c.Proposal)
            .Include(c => c.Author)
            .Include(c => c.Links)
            .Include(c => c.Skills)
            .Include(c => c.Invitions)
            .Include(c => c.Sessions)
            .Include(c => c.TodoTasks)
            .Include(c => c.Upvotes)
            .SingleOrDefaultAsync(c => c.Id == circleId && c.Status == CircleStatusEnum.Active, ct) ??
            throw new ArgumentException("Invalid Circle Id");

        // if Circle has an accepted , throw exception cuz it can't be deleted
        if (circle.Proposal is not null &&
                await db.ApprovalRequests.AnyAsync(ar =>
                    ar.ProposalId == circle.Proposal.Id &&
                    ar.ProposalStatusSnapshot == ProposalStatusEnum.SecondApproval &&
                    ar.IsAccepted == true, ct))
            throw new InvalidOperationException("Cannot delete a circle with accepted third approval request");

        if (!circle.Invitions.IsNullOrEmpty()) db.JoinRequests.RemoveRange(circle.Invitions);

        if (circle.Proposal is not null &&
                await db.ApprovalRequests.AnyAsync(ar =>
                    ar.ProposalId == circle.Proposal.Id &&
                    ar.IsAccepted == true, ct))
        {
            circle.Status = CircleStatusEnum.Deleted;
            return;
        }

        db.Permissions.Remove(circle.DefaultPermission);

        if (!circle.CircleMembers.IsNullOrEmpty()) db.CircleMembers.RemoveRange(circle.CircleMembers);

        if (!circle.Links.IsNullOrEmpty()) db.Links.RemoveRange(circle.Links);

        if (!circle.Skills.IsNullOrEmpty()) db.Skills.RemoveRange(circle.Skills);

        if (!circle.Upvotes.IsNullOrEmpty()) db.Upvotes.RemoveRange(circle.Upvotes);

        if (!circle.TodoTasks.IsNullOrEmpty()) db.TodoTasks.RemoveRange(circle.TodoTasks);

        if (!circle.Sessions.IsNullOrEmpty()) db.Sessions.RemoveRange(circle.Sessions);

        if (circle.Proposal is not null) db.Proposals.Remove(circle.Proposal);

        if (circle.Author is not null) db.Authors.Remove(circle.Author);

        if (!circle.CircleMembers.Where(cm => cm.ExceptionPermission != null).IsNullOrEmpty())
            db.Permissions.RemoveRange(
                circle.CircleMembers
                    .Select(cm => cm.ExceptionPermission)
                    .Where(p => p != null)!);

        db.Circles.Remove(circle);

        await db.SaveChangesAsync(ct);
    }
}