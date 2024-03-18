using TeamsMaker.Api.Contracts.Requests.TodoTask;

namespace TeamsMaker.Api.Services.Circles.Interfaces;

public interface ITodoTaskService
{
    Task<Guid> AddAsync(AddTodoTaskRequest request, CancellationToken ct);
    Task UpdateInfoAsync(Guid id, UpdateTodoTaskInfoRequest request, CancellationToken ct);
    Task UpdateStatusAsync(Guid id, TodoTaskStatus status, CancellationToken ct);
    Task DeleteAsync(Guid id, CancellationToken ct);
}
