namespace TeamsMaker.Api.Contracts.Requests.TodoTask;

public class UpdateTodoTaskInfoRequest
{
    public string? Title { get; init; }
    public string? Notes { get; init; }
    public DateOnly? DeadLine { get; init; }
    public Guid? SessionId { get; init; }
}
