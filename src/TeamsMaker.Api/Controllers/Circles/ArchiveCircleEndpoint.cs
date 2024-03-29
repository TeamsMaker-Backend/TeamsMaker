using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Services.Circles.Interfaces;

namespace TeamsMaker.Api.Controllers.Circles;

[Authorize]
public class ArchiveCircleEndpoint(ICircleService circleService) : BaseApiController
{
    [Tags("circles/danger_zone")]
    [HttpPut("circles/{circleId}/archive")]
    public async Task<IActionResult> ArchiveCircle(Guid circleId, CancellationToken ct)
    {
        try
        {
            await circleService.ArchiveAsync(circleId, ct);
        }
        catch (ArgumentException e)
        {
            return NotFound(_response.FailureResponse(e.Message));
        }

        return Ok();
    }
}
