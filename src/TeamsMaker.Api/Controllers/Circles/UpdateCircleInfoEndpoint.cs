using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Contracts.Requests.Circle;
using TeamsMaker.Api.Services.Circles.Interfaces;

namespace TeamsMaker.Api.Controllers.Circles;

[Authorize]
public class UpdateCircleInfoEndpoint(ICircleService circleService) : BaseApiController
{
    [Tags("circles")]
    [HttpPut("circles/info/{id}")]
    public async Task<IActionResult> CircleLinks(Guid id, UpdateCircleInfoRequest request, CancellationToken ct)
    {
        try
        {
            await circleService.UpdateInfoAsync(id, request, ct);
        }
        catch (ArgumentException e)
        {
            return NotFound(_response.FailureResponse(e.Message));
        }
        return Ok();
    }
}
