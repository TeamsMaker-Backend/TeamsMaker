
namespace TeamsMaker.Api.Contracts.Responses.JoinRequest;

public class GetBaseJoinRequestResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!; // circle name
    public string? Avatar { get; set; } // circle avatar || student avatar
    public string Sender { get; set; } = null!;
}