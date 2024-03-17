using TeamsMaker.Api.Contracts.Requests.TodoTask;

namespace TeamsMaker.Api.Services.Circles.Interfaces;

public interface ITodoTaskService
{
    Task AddAsync(AddTodoTaskRequest request, CancellationToken ct);
}
