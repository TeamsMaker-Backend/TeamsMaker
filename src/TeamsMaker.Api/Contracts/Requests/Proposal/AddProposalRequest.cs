namespace TeamsMaker.Api.Contracts.Requests.Proposal;

public class AddProposalRequest
{
    public Guid CircleId { get; init; }
    public required string Overview { get; init; }
    public required string Objectives { get; init; }
    public required string TechStack { get; init; }
}
