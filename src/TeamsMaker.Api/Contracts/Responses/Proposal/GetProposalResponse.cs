using TeamsMaker.Api.Core.Enums;

namespace TeamsMaker.Api.Contracts.Responses.Proposal;

public class GetProposalResponse
{
    public Guid Id { get; set; }
    public string Overview { get; set; } = null!;
    public string Objectives { get; set; } = null!;
    public string TechStack { get; set; } = null!;
    public bool IsRested { get; set; }
    public ProposalStatusEnum Status { get; set; } = ProposalStatusEnum.NoApproval;
    public bool IsReseted { get; set; }
    public Guid CircleId { get; set; }
}
