using TeamsMaker.Api.Core.Enums;

namespace TeamsMaker.Api.Contracts.QueryStringParameters;

public class ApprovalRequestQueryString
{
    public PositionEnum? PositionEnum { get; init; }
    public ProposalStatusEnum? ProposalStatusEnum { get; init; }
}
