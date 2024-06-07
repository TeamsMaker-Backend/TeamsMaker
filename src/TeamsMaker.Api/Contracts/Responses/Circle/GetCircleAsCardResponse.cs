using TeamsMaker.Api.Contracts.Responses.Profile;

namespace TeamsMaker.Api.Contracts.Responses.Circle;

public class GetCircleAsCardResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string OwnerName { get; set; } = null!;
    public string? Summary { get; set; }
    public string? Avatar { get; set; }
    public string? Keywords { get; set; }
    public long Rate { get; set; }
    public ICollection<LinkInfo> Links { get; set; } = [];
}