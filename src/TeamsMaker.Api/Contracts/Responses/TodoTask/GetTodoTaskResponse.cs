namespace TeamsMaker.Api.Contracts.Responses.TodoTask;

public class GetTodoTaskResponse
{
    public Guid Id { get; set; }
    public string CreatedBy { get; set; } = null!;
    public DateTime? CreationDate { get; set; }

    public string Title { get; set; } = null!;
    public string? Notes { get; set; }
    public TodoTaskStatus Status { get; set; }
    public DateOnly DeadLine { get; set; }

    public Guid? SessionId { get; set; }
}
