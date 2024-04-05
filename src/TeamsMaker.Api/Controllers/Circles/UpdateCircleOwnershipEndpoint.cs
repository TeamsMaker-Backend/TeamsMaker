using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Services.Circles.Interfaces;

namespace TeamsMaker.Api.Controllers.Circles;

[Authorize]
public class UpdateCircleOwnershipEndpoint(ICircleService circleService) : BaseApiController
{
    [Tags("circles/danger_zone")]
    [HttpPut("circles/{circleId}/ownership/{newOwnerId}")]
    public async Task<IActionResult> CircleOwnership(Guid circleId, string newOwnerId, CancellationToken ct)
    {
        try
        {
            await circleService.TransferOwnershipAsync(circleId, newOwnerId, ct);
        }
        catch (ArgumentException e)
        {
            return NotFound(_response.FailureResponse(e.Message));
        }

        return Ok(_response.SuccessResponse(null));
    }
}
