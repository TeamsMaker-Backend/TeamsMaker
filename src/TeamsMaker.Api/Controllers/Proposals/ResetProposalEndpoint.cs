using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Services.Proposals.Interfaces;

namespace TeamsMaker.Api.Controllers.Proposals;

[Authorize]
public class ResetProposalEndpoint(IProposalService proposalService) : BaseApiController
{
    [Tags("proposal")]
    [HttpPatch("proposal/{id}/reset")]
    public async Task<IActionResult> Reset(Guid id, CancellationToken ct)
    {
        try
        {
            await proposalService.ResetAsync(id, ct);
        }
        catch (Exception e)
        {
            return NotFound(_response.FailureResponse(e.Message));
        }

        return Ok(_response.SuccessResponse(null));
    }
}
