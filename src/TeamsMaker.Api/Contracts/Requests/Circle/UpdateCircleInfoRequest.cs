namespace TeamsMaker.Api.Contracts.Requests.Circle;

public class UpdateCircleInfoRequest
{
    public required string Name { get; init; }
    public string? Description { get; init; }
    public string? Summary { get; init; }
    public ICollection<string>? Skills { get; init; }
}
