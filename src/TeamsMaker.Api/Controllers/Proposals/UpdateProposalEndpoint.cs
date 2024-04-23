using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Contracts.Requests.Proposal;
using TeamsMaker.Api.Services.Proposals.Interfaces;

namespace TeamsMaker.Api.Controllers.Proposals;

[Authorize]
public class UpdateProposalEndpoint(IProposalService proposalService) : BaseApiController
{
    [Tags("proposal")]
    [HttpPatch("proposal/{id}")]
    public async Task<IActionResult> Proposal(Guid id, UpdateProposalRequest request, CancellationToken ct)
    {
        try
        {
            await proposalService.UpdateAsync(id, request, ct);
        }
        catch (Exception e)
        {
            return NotFound(_response.FailureResponse(e.Message));
        }

        return Ok(_response.SuccessResponse(null));
    }
}
