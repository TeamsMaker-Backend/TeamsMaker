using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


using TeamsMaker.Api.Services.Proposals.Interfaces;

namespace TeamsMaker.Api.Controllers.Proposals;

[Authorize]
public class DeleteProposalEndpoint(IProposalService proposalService) : BaseApiController
{
    [Tags("proposal")]
    [HttpDelete("proposal/{id}")]
    public async Task<IActionResult> Proposal(Guid id, CancellationToken ct)
    {
        try
        {
            await proposalService.DeleteAsync(id, ct);
        }
        catch (Exception e)
        {
            return NotFound(_response.FailureResponse(e.Message));
        }

        return Ok(_response.SuccessResponse(null));
    }
}
