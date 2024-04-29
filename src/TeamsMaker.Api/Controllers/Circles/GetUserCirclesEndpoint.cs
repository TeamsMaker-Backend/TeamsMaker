using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Contracts.Responses.Circle;
using TeamsMaker.Api.Services.Circles.Interfaces;

namespace TeamsMaker.Api.Controllers.Circles;

[Authorize]
public class GetUserCirclesEndpoint(ICircleService circleService) : BaseApiController
{
    [Tags("circles")]
    [Produces<List<GetCircleAsRowResponse>>]
    [HttpGet("circles/my")]
    public async Task<IActionResult> Circle(CancellationToken ct)
    {
        List<GetCircleAsRowResponse> response;

        try
        {
            response = await circleService.GetAsync(ct);
        }
        catch (Exception e)
        {
            return NotFound(_response.FailureResponse(e.Message));
        }

        return Ok(_response.SuccessResponse(response));
    }
}
