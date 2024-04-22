using TeamsMaker.Api.Contracts.Requests.Proposal;
using TeamsMaker.Api.Contracts.Responses.Proposal;

namespace TeamsMaker.Api.Services.Proposals.Interfaces;

public interface IProposalService
{
    Task<GetProposalResponse> GetAsync(Guid circleId, CancellationToken ct);
    Task<Guid> AddAsync(AddProposalRequest request, CancellationToken ct);
    Task UpdateAsync(Guid id, UpdateProposalRequest request, CancellationToken ct);
    Task ResetAsync(Guid id, CancellationToken ct);
    Task DeleteAsync(Guid id, CancellationToken ct);
}

/* Delete

NoApproval: valid

1st: approval request true -> valid

2nd: ar + supervisor -> valid

3rd -> not valid
*/


/* Update

proposal.Status ? No -> update

Reset? isAccepted = false | Supervisor = null | 

third approval -> no reset
*/