using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Services.Circles.Interfaces;

namespace TeamsMaker.Api.Controllers.Circles;

[Authorize]
public class UpdateCircleOwnershipEndpoint(ICircleService circleService) : BaseApiController
{
    [Tags("circles/permissions/danger_zone")]
    [HttpPut("circles/{circleId}/ownership/{newOwnerID}")]
    public async Task<IActionResult> CircleOwnership(Guid circleId, string newOwnerID, CancellationToken ct)
    {
        try
        {
            await circleService.ChangeOwnershipAsync(circleId, newOwnerID, ct);
        }
        catch (ArgumentException e)
        {
            return NotFound(_response.FailureResponse(e.Message));
        }

        return Ok();
    }
}
