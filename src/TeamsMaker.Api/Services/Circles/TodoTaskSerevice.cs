using TeamsMaker.Api.Contracts.Requests.TodoTask;
using TeamsMaker.Api.DataAccess.Context;
using TeamsMaker.Api.Services.Circles.Interfaces;

namespace TeamsMaker.Api.Controllers.Circles;

public class TodoTaskSerevice(AppDBContext db) : ITodoTaskService
{
    public async Task AddAsync(AddTodoTaskRequest request, CancellationToken ct)
    {
        var circle = await db.Circles
            .FindAsync([request.CircleId], ct) ??
            throw new ArgumentException("Invalid Circle ID");

        circle.TodoTasks.Add(new TodoTask
        {
            Title = request.Title,
            Notes = request.Notes,
            Status = request.Status ?? TodoTaskStatus.NotStarted,
            DeadLine = request.DeadLine,

            CircleId = request.CircleId,
            SessionId = request.SessionId
        });

        await db.SaveChangesAsync(ct);
    }
}
