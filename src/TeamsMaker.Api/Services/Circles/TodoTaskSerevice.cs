using Core.Generics;

using DataAccess.Base.Interfaces;

using TeamsMaker.Api.Contracts.QueryStringParameters;
using TeamsMaker.Api.Contracts.Requests.TodoTask;
using TeamsMaker.Api.Contracts.Responses.TodoTask;
using TeamsMaker.Api.Core.Enums;
using TeamsMaker.Api.DataAccess.Context;
using TeamsMaker.Api.Services.Circles.Interfaces;

namespace TeamsMaker.Api.Controllers.Circles;

public class TodoTaskSerevice
    (AppDBContext db, IUserInfo userInfo, ICircleValidationService validationService) : ITodoTaskService
{
    public async Task<Guid> AddAsync(Guid circleId, AddTodoTaskRequest request, CancellationToken ct)
    {
        var circle = await db.Circles
            .Include(c => c.DefaultPermission)
            .Include(c => c.Sessions)
            .SingleOrDefaultAsync(c => c.Id == circleId, ct) ??
            throw new ArgumentException("Invalid Circle ID");

        if (request.SessionId != null && !circle.Sessions.Any(s => s.Id == request.SessionId))
            throw new ArgumentException("Invalid Session ID");

        var circleMember = await validationService.TryGetCircleMemberAsync(userInfo.UserId, circleId, ct);

        validationService.CheckPermission(circleMember, circle, PermissionsEnum.TodoTaskManagement);

        var todoTask = new TodoTask
        {
            Title = request.Title,
            Notes = request.Notes,
            Status = request.Status ?? TodoTaskStatus.NotStarted,
            DeadLine = request.DeadLine,

            SessionId = request.SessionId
        };

        circle.TodoTasks.Add(todoTask);

        await db.SaveChangesAsync(ct);

        return todoTask.Id;
    }

    public async Task<PagedList<GetTodoTaskResponse>> ListAsync(Guid circleId, TodoTaskQueryString queryString, CancellationToken ct)
    {
        var todoTasks = db.TodoTasks
            .Include(td => td.Session)
            .Where(td => td.CircleId == circleId &&
                (queryString.Status == null || td.Status == queryString.Status))
            .OrderBy(td => td.DeadLine)
            .Select(td => new GetTodoTaskResponse
            {
                Id = td.Id,
                CreatedBy = td.CreatedBy,
                CreationDate = td.CreationDate,

                Title = td.Title,
                Notes = td.Notes,
                Status = td.Status,
                DeadLine = td.DeadLine,

                SessionId = td.SessionId
            });

        return await PagedList<GetTodoTaskResponse>
                        .ToPagedListAsync(todoTasks, queryString.PageNumber, queryString.PageSize, ct);
    }

    public async Task UpdateInfoAsync(Guid id, UpdateTodoTaskInfoRequest request, CancellationToken ct)
    {
        var todoTask = await db.TodoTasks
            .Include(td => td.Circle)
                .ThenInclude(c => c.DefaultPermission)
            .SingleOrDefaultAsync(td => td.Id == id, ct) ??
            throw new ArgumentException("Invalid Todo Task Id");

        var circleMember = await validationService.TryGetCircleMemberAsync(userInfo.UserId, todoTask.CircleId, ct);

        validationService.CheckPermission(circleMember, todoTask.Circle, PermissionsEnum.TodoTaskManagement);

        if (!string.IsNullOrEmpty(request.Title)) todoTask.Title = request.Title;
        if (request.DeadLine.HasValue) todoTask.DeadLine = request.DeadLine.Value;
        
        todoTask.Notes = request.Notes;
        todoTask.SessionId = request.SessionId;

        await db.SaveChangesAsync(ct);
    }

    public async Task UpdateStatusAsync(Guid id, TodoTaskStatus status, CancellationToken ct)
    {
        var todoTask = await db.TodoTasks
            .Include(td => td.Circle)
                .ThenInclude(c => c.DefaultPermission)
            .SingleOrDefaultAsync(td => td.Id == id, ct) ??
            throw new ArgumentException("Invalid Todo Task Id");

        var circleMember = await validationService.TryGetCircleMemberAsync(userInfo.UserId, todoTask.CircleId, ct);

        validationService.CheckPermission(circleMember, todoTask.Circle, PermissionsEnum.TodoTaskManagement);

        todoTask.Status = status;

        await db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct)
    {
        var todoTask = await db.TodoTasks
            .Include(td => td.Circle)
                .ThenInclude(c => c.DefaultPermission)
            .SingleOrDefaultAsync(td => td.Id == id, ct) ??
            throw new ArgumentException("Invalid Todo Task Id");

        var circleMember = await validationService.TryGetCircleMemberAsync(userInfo.UserId, todoTask.CircleId, ct);

        validationService.CheckPermission(circleMember, todoTask.Circle, PermissionsEnum.TodoTaskManagement);

        db.TodoTasks.Remove(todoTask);

        await db.SaveChangesAsync(ct);
    }
}
