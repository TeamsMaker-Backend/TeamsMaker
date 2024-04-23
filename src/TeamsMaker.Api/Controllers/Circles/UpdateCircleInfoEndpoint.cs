using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Contracts.Requests.Circle;
using TeamsMaker.Api.Services.Circles.Interfaces;

namespace TeamsMaker.Api.Controllers.Circles;

[Authorize]
public class UpdateCircleInfoEndpoint(ICircleService circleService) : BaseApiController
{
    [Tags("circles")]
    [HttpPatch("circles/{circleId}/info")]
    public async Task<IActionResult> CircleInfo(Guid circleId, UpdateCircleInfoRequest request, CancellationToken ct)
    {
        try
        {
            await circleService.UpdateInfoAsync(circleId, request, ct);
        }
        catch (Exception e)
        {
            return NotFound(_response.FailureResponse(e.Message));
        }

        return Ok(_response.SuccessResponse(null));
    }
}
