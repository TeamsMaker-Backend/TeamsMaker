namespace TeamsMaker.Api.Contracts.Responses.JoinRequest;

public class GetJoinRequestResponse
{
    public ICollection<GetBaseJoinRequestResponse> JoinRequests { get; set; } = [];
    public ICollection<GetBaseJoinRequestResponse> Invitations { get; set; } = [];
}
