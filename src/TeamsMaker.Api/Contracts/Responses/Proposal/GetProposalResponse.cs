using TeamsMaker.Api.Core.Enums;

namespace TeamsMaker.Api.Contracts.Responses.Proposal;

public class GetProposalResponse
{
    public Guid Id { get; set; }
    public string Overview { get; set; } = null!;
    public string Objectives { get; set; } = null!;
    public string TechStack { get; set; } = null!;
    public ProposalStatusEnum Status { get; set; } = ProposalStatusEnum.NoApproval;

    public Guid CircleId { get; set; }
}
