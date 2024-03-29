namespace TeamsMaker.Api.Contracts.Requests.Session;

public class AddSessionRequest
{
    public required string Title { get; init; }
    public string? Notes { get; init; }
    public SessionStatus? Status { get; init; }
    public required DateOnly Date { get; init; }
    public TimeOnly? Time { get; init; }
}
