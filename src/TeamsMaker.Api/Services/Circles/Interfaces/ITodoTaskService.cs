using Core.Generics;

using TeamsMaker.Api.Contracts.QueryStringParameters;
using TeamsMaker.Api.Contracts.Requests.TodoTask;
using TeamsMaker.Api.Contracts.Responses.TodoTask;

namespace TeamsMaker.Api.Services.Circles.Interfaces;

public interface ITodoTaskService
{
    Task<Guid> AddAsync(Guid circleId, AddTodoTaskRequest request, CancellationToken ct);
    Task<PagedList<GetTodoTaskResponse>> ListAsync(Guid circleId, TodoTaskStatus? status, TodoTaskQueryString queryString, CancellationToken ct);
    Task UpdateInfoAsync(Guid id, UpdateTodoTaskInfoRequest request, CancellationToken ct);
    Task UpdateStatusAsync(Guid id, TodoTaskStatus status, CancellationToken ct);
    Task DeleteAsync(Guid id, CancellationToken ct);
}
