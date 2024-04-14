
namespace TeamsMaker.Api.Contracts.Responses.JoinRequest;

public class GetBaseJoinRequestResponse
{
    public Guid JoinRequestId { get; set; }
    public string OtherSideId { get; set; } = null!;
    public string OtherSideName { get; set; } = null!; // other side name
    public string? Avatar { get; set; } // circle avatar || student avatar
    public string Sender { get; set; } = null!;
}