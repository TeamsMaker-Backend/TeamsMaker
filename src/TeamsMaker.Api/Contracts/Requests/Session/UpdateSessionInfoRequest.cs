namespace TeamsMaker.Api.Contracts.Requests.Session;

public class UpdateSessionInfoRequest
{
    public string? Title { get; init; }
    public string? Notes { get; init; }
    public DateOnly? Date { get; init; }
    public TimeOnly? Time { get; init; }
}
