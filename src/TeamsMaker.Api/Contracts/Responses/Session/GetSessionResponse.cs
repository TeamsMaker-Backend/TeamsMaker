namespace TeamsMaker.Api.Contracts.Responses.Session;

public class GetSessionResponse
{
    public Guid Id { get; set; }
    public string CreatedBy { get; set; } = null!;
    public DateTime? CreationDate { get; set; }

    public string Title { get; set; } = null!;
    public string? Notes { get; set; }
    public SessionStatus Status { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly? Time { get; set; }
}
