using TeamsMaker.Api.Core.Enums;

namespace TeamsMaker.Api.Contracts.QueryStringParameters;

public class ApprovalRequestQueryString
{
    public bool? IsAccepted { get; init; }
    public PositionEnum? PositionEnum { get; init; }
    public ProposalStatusEnum? ProposalStatusEnum { get; init; }
}
