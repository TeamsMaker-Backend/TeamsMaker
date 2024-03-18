namespace TeamsMaker.Api.Contracts.Requests.Session;

public class UpdateSessionInfoRequest
{
    public required string Title { get; init; }
    public string? Notes { get; init; }
    public required DateOnly Date { get; init; }
    public TimeOnly? Time { get; init; }
}
