using Core.Generics;

using TeamsMaker.Api.Contracts.QueryStringParameters;
using TeamsMaker.Api.Contracts.Requests.TodoTask;
using TeamsMaker.Api.Contracts.Responses.TodoTask;
using TeamsMaker.Api.DataAccess.Context;
using TeamsMaker.Api.Services.Circles.Interfaces;

namespace TeamsMaker.Api.Controllers.Circles;

public class TodoTaskSerevice(AppDBContext db) : ITodoTaskService
{
    public async Task<Guid> AddAsync(AddTodoTaskRequest request, CancellationToken ct)
    {
        var circle = await db.Circles
            .FindAsync([request.CircleId], ct) ??
            throw new ArgumentException("Invalid Circle ID");

        var todoTask = new TodoTask
        {
            Title = request.Title,
            Notes = request.Notes,
            Status = request.Status ?? TodoTaskStatus.NotStarted,
            DeadLine = request.DeadLine,

            CircleId = request.CircleId,
            SessionId = request.SessionId
        };

        circle.TodoTasks.Add(todoTask);

        await db.SaveChangesAsync(ct);

        return todoTask.Id;
    }

    public async Task<PagedList<GetTodoTaskResponse>> ListAsync(Guid circleId, TodoTaskStatus? status, TodoTaskQueryString queryString, CancellationToken ct)
    {
        var todoTasks = db.TodoTasks
            .Include(td => td.Session)
            .Where(td => td.CircleId == circleId)
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

        if (status is not null)
            todoTasks = todoTasks.Where(td => td.Status == status);

        return await PagedList<GetTodoTaskResponse>
                        .ToPagedListAsync(todoTasks, queryString.PageNumber, queryString.PageSize, ct);
    }

    public async Task UpdateInfoAsync(Guid id, UpdateTodoTaskInfoRequest request, CancellationToken ct)
    {
        var todoTask = await db.TodoTasks.FindAsync([id], ct) ??
            throw new ArgumentException("Invalid Todo Task Id");

        todoTask.Title = request.Title;
        todoTask.Notes = request.Notes;
        todoTask.DeadLine = request.DeadLine;
        todoTask.SessionId = request.SessionId;

        await db.SaveChangesAsync(ct);
    }

    public async Task UpdateStatusAsync(Guid id, TodoTaskStatus status, CancellationToken ct)
    {
        var todoTask = await db.TodoTasks
            .SingleOrDefaultAsync(td => td.Id == id, ct) ??
            throw new ArgumentException("Invalid Todo Task Id");

        todoTask.Status = status;

        await db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct)
    {
        var todoTask = await db.TodoTasks.FindAsync([id], ct) ??
            throw new ArgumentException("Invalid Todo Task Id");

        db.TodoTasks.Remove(todoTask);

        await db.SaveChangesAsync(ct);
    }
}
