namespace TeamsMaker.Api.Contracts.Requests.Circle;

public class UpdateDeafultPermissionRequest
{
    public bool MemberManagement { get; init; }
    public bool CircleManagment { get; init; }
    public bool ProposalManagment { get; init; }
    public bool FeedManagment { get; init; }
}
