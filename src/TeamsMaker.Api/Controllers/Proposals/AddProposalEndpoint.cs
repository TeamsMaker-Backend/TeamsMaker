using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Contracts.Requests.Proposal;

using TeamsMaker.Api.Contracts.Responses;
using TeamsMaker.Api.Services.Proposals.Interfaces;

namespace TeamsMaker.Api.Controllers.Proposals;

[Authorize]
public class AddProposalEndpoint(IProposalService proposalService) : BaseApiController
{
    [Tags("proposal")]
    [Produces<IdResponse<Guid>>]
    [HttpPost("proposal")]
    public async Task<IActionResult> Proposal(AddProposalRequest request, CancellationToken ct)
    {
        Guid proposalId;

        try
        {
            proposalId = await proposalService.AddAsync(request, ct);
        }
        catch (Exception e)
        {
            return NotFound(_response.FailureResponse(e.Message));
        }

        return Created("", _response.SuccessResponse(new IdResponse<Guid>(proposalId)));
    }
}
