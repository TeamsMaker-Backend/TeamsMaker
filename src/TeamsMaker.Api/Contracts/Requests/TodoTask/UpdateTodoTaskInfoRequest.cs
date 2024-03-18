namespace TeamsMaker.Api.Contracts.Requests.TodoTask;

public class UpdateTodoTaskInfoRequest
{
    public required string Title { get; init; }
    public string? Notes { get; init; }
    public required DateOnly DeadLine { get; init; }
    public Guid? SessionId { get; init; }
}
