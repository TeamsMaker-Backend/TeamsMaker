using TeamsMaker.Api.Contracts.Requests.Proposal;
using TeamsMaker.Api.Contracts.Responses.Proposal;

namespace TeamsMaker.Api.Services.Proposals.Interfaces;

public interface IProposalService
{
    Task<GetProposalResponse> GetAsync(Guid circleId, CancellationToken ct);
    Task<Guid> AddAsync(AddProposalRequest request, CancellationToken ct);
    Task UpdateAsync(Guid id, UpdateProposalRequest request, CancellationToken ct);
    Task DeleteAsync(Guid id, CancellationToken ct);
}
/* Delete

*/


/* Update

proposal.Status ? No -> update : no updates ?? 
Reset? isAccepted = false | Supervisor = null | 
third approval ? 

*/