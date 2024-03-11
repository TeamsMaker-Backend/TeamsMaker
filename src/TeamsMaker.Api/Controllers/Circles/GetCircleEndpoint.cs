using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Contracts.Responses.Circle;
using TeamsMaker.Api.Services.Circles.Interfaces;

namespace TeamsMaker.Api.Controllers.Circles;

[Authorize]
public class GetCircleEndpoint(ICircleService circleService) : BaseApiController
{
    [Tags("circles")]
    [HttpGet("circles/{id}")]
    public async Task<IActionResult> Circle(Guid id, CancellationToken ct)
    {
        GetCircleResponse response;

        try
        {
            response = await circleService.GetAsync(id, ct);
        }
        catch (ArgumentException e)
        {
            return NotFound(_response.FailureResponse(e.Message));
        }

        return Ok(_response.SuccessResponse(response));
    }
}
