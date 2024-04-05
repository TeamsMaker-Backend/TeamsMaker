using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Services.Circles.Interfaces;

namespace TeamsMaker.Api.Controllers.Circles.Upvotes;

[Authorize]
public class DownvoteEndpoint(ICircleService circleService) : BaseApiController
{
    [Tags("circles/upvote")]
    [HttpPost("circles/{circleId}/downvote")]
    public async Task<IActionResult> Downvote(Guid circleId, CancellationToken ct)
    {
        try
        {
            await circleService.DownvoteAsync(circleId, ct);
        }
        catch (ArgumentException e)
        {
            return NotFound(_response.FailureResponse(e.Message));
        }

        return Ok(_response.SuccessResponse(null));
    }
}
