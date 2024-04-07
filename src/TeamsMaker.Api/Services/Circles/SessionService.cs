using Core.Generics;

using DataAccess.Base.Interfaces;

using TeamsMaker.Api.Contracts.QueryStringParameters;
using TeamsMaker.Api.Contracts.Requests.Session;
using TeamsMaker.Api.Contracts.Responses.Session;
using TeamsMaker.Api.Core.Enums;
using TeamsMaker.Api.DataAccess.Context;
using TeamsMaker.Api.Services.Circles.Interfaces;

namespace TeamsMaker.Api.Services.Circles;

public class SessionService
    (AppDBContext db, IUserInfo userInfo, ICircleValidationService validationService) : ISessionService
{
    public async Task<Guid> AddAsync(Guid circleId, AddSessionRequest request, CancellationToken ct)
    {
        var circle = await db.Circles
            .Include(c => c.DefaultPermission)
            .SingleOrDefaultAsync(c => c.Id == circleId, ct) ??
            throw new ArgumentException("Invalid Circle ID");

        var circleMember = await validationService.TryGetCircleMemberAsync(userInfo.UserId, circleId, ct);

        validationService.CheckPermission(circleMember, circle, PermissionsEnum.SessionManagement);

        var session = new Session
        {
            Title = request.Title,
            Notes = request.Notes,
            Status = request.Status ?? SessionStatus.Upcoming,
            Date = request.Date,
            Time = request.Time,
        };

        circle.Sessions.Add(session);

        await db.SaveChangesAsync(ct);

        return session.Id;
    }

    public async Task<PagedList<GetSessionResponse>> ListAsync(Guid circleId, SessionsQueryString queryString, CancellationToken ct)
    {
        var sessions = db.Sessions
            .Where(s => s.CircleId == circleId && (queryString.Status == null || s.Status == queryString.Status))
            .OrderBy(s => s.Date)
            .ThenBy(s => s.Time)
            .Select(s => new GetSessionResponse
            {
                Id = s.Id,
                CreatedBy = s.CreatedBy,
                CreationDate = s.CreationDate,

                Title = s.Title,
                Notes = s.Notes,
                Status = s.Status,
                Date = s.Date,
                Time = s.Time
            });

        return await PagedList<GetSessionResponse>
                        .ToPagedListAsync(sessions, queryString.PageNumber, queryString.PageSize, ct);
    }

    public async Task UpdateInfoAsync(Guid id, UpdateSessionInfoRequest request, CancellationToken ct)
    {
        var session = await db.Sessions
            .Include(s => s.Circle)
                .ThenInclude(c => c.DefaultPermission)
            .SingleOrDefaultAsync(s => s.Id == id, ct) ??
            throw new ArgumentException("Invalid Session Id");

        var circleMember = await validationService.TryGetCircleMemberAsync(userInfo.UserId, session.CircleId, ct);

        validationService.CheckPermission(circleMember, session.Circle, PermissionsEnum.SessionManagement);

        if (!string.IsNullOrEmpty(request.Title)) session.Title = request.Title;
        if (request.Date.HasValue) session.Date = request.Date.Value;
        
        session.Notes = request.Notes;
        session.Time = request.Time;

        await db.SaveChangesAsync(ct);
    }

    public async Task UpdateStatusAsync(Guid id, SessionStatus status, CancellationToken ct)
    {
        var session = await db.Sessions
            .Include(s => s.Circle)
                .ThenInclude(c => c.DefaultPermission)
            .SingleOrDefaultAsync(s => s.Id == id, ct) ??
            throw new ArgumentException("Invalid Session Id");

        var circleMember = await validationService.TryGetCircleMemberAsync(userInfo.UserId, session.CircleId, ct);

        validationService.CheckPermission(circleMember, session.Circle, PermissionsEnum.SessionManagement);

        session.Status = status;

        await db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct)
    {
        var session = await db.Sessions
            .Include(s => s.Circle)
                .ThenInclude(c => c.DefaultPermission)
            .Include(s => s.TodoTasks)
            .SingleOrDefaultAsync(s => s.Id == id, ct) ??
            throw new ArgumentException("Invalid Session Id");

        var circleMember = await validationService.TryGetCircleMemberAsync(userInfo.UserId, session.CircleId, ct);

        validationService.CheckPermission(circleMember, session.Circle, PermissionsEnum.SessionManagement);

        session.TodoTasks = [];
        db.Sessions.Remove(session);

        await db.SaveChangesAsync(ct);
    }
}
