
namespace TeamsMaker.Api.Contracts.Responses.JoinRequest;

public class GetCircleJoinRequestResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!; // circle name
    public string? Avatar { get; set; } // circle avatar || student avatar
    public Guid CircleId { get; set; }
}