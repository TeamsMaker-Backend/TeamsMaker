using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Contracts.Requests.Circle;
using TeamsMaker.Api.Services.Circles.Interfaces;

namespace TeamsMaker.Api.Controllers.Circles;

public class UpdateCircleLinksEndpoint(ICircleService circleService) : BaseApiController
{
    [Tags("circles")]
    [HttpPut("circles/links{id}")]
    public async Task<IActionResult> CircleLinks(Guid id, UpdateCircleLinksRequest request, CancellationToken ct)
    {
        try
        {
            await circleService.UpdateLinksAsync(id, request, ct);
        }
        catch (ArgumentException e)
        {
            return NotFound(_response.FailureResponse(e.Message));
        }
        return Ok();
    }
}
