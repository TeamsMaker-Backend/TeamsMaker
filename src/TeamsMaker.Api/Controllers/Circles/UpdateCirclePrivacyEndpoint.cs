using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Services.Circles.Interfaces;

namespace TeamsMaker.Api.Controllers.Circles;

[Authorize]
public class UpdateCirclePrivacyEndpoint(ICircleService circleService) : BaseApiController
{
    [Tags("circles/permissions/circle_management")]
    [HttpPut("circles/{circleId}/privacies/{isPublic}")]
    public async Task<IActionResult> CirclePrivacy(Guid circleId, bool isPublic, CancellationToken ct)
    {
        try
        {
            await circleService.UpdatePrivacyAsync(circleId, isPublic, ct);
        }
        catch (ArgumentException e)
        {
            return NotFound(_response.FailureResponse(e.Message));
        }
        return Ok();
    }
}
