namespace TeamsMaker.Api.Contracts.Responses.Circle;

public class GetCircleAsRowResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Avatar { get; set; }
    public string OwnerName { get; set; } = null!;
}