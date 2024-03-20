using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Contracts.Requests.Circle;
using TeamsMaker.Api.Services.Circles.Interfaces;

namespace TeamsMaker.Api.Controllers.Circles;

[Authorize]
public class UpdateCircleDefaultPermissionEndpoint(ICircleService circleService) : BaseApiController
{
    [Tags("circles/permissions/danger_zone")]
    [HttpPut("circles/{circleId}/default_permission")]
    public async Task<IActionResult> CircleDefaultPermission(Guid circleId, UpdateDeafultPermissionRequest request, CancellationToken ct)
    {
        try
        {
            await circleService.UpdateDefaultPermissionAsync(circleId, request, ct);
        }
        catch (ArgumentException e)
        {
            return NotFound(_response.FailureResponse(e.Message));
        }

        return Ok();
    }
}
