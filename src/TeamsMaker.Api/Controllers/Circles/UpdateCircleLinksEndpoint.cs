using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Contracts.Requests.Circle;
using TeamsMaker.Api.Services.Circles.Interfaces;

namespace TeamsMaker.Api.Controllers.Circles;

[Authorize]
public class UpdateCircleLinksEndpoint(ICircleService circleService) : BaseApiController
{
    [Tags("circles/permissions/circle_management")]
    [HttpPut("circles/{circleId}/links")]
    public async Task<IActionResult> CircleLinks(Guid circleId, UpdateCircleLinksRequest request, CancellationToken ct)
    {
        try
        {
            await circleService.UpdateLinksAsync(circleId, request, ct);
        }
        catch (ArgumentException e)
        {
            return NotFound(_response.FailureResponse(e.Message));
        }
        return Ok();
    }
}
