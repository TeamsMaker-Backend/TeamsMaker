using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Swashbuckle.AspNetCore.Annotations;


using TeamsMaker.Api.Contracts.Responses.Proposal;
using TeamsMaker.Api.Services.Proposals.Interfaces;

namespace TeamsMaker.Api.Controllers.Proposals;

[Authorize]
public class GetProposalEndpoint(IProposalService proposalService) : BaseApiController
{
    [Tags("proposal")]
    [Produces<GetProposalResponse>]
    [SwaggerOperation(Summary = "if there is no proposal, will return null")]
    [HttpGet("proposal/{circleId}")]
    public async Task<IActionResult> Proposal(Guid circleId, CancellationToken ct)
    {
        GetProposalResponse? response;

        try
        {
            response = await proposalService.GetAsync(circleId, ct);
        }
        catch (Exception e)
        {
            return NotFound(_response.FailureResponse(e.Message));
        }

        return Created("", _response.SuccessResponse(response));
    }
}
