using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Services.Circles.Interfaces;

namespace TeamsMaker.Api.Controllers.Circles.Upvotes;

[Authorize]
public class UpvoteEndpoint(ICircleService circleService) : BaseApiController
{
    [Tags("circles/upvote")]
    [HttpPost("circles/{circleId}/upvote")]
    public async Task<IActionResult> Upvote(Guid circleId, CancellationToken ct)
    {
        try
        {
            await circleService.UpvoteAsync(circleId, ct);
        }
        catch (Exception e)
        {
            return NotFound(_response.FailureResponse(e.Message));
        }

        return Ok(_response.SuccessResponse(null));
    }
}
