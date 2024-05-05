using Core.Generics;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Contracts.QueryStringParameters;

using TeamsMaker.Api.Contracts.Responses.Circle;
using TeamsMaker.Api.Services.Circles.Interfaces;

namespace TeamsMaker.Api.Controllers.Circles;

[Authorize]
public class GetPaginatedCirclesEndpoint(ICircleService circleService) : BaseApiController
{
    [Tags("circles")]
    [Produces<PagedList<GetCircleAsRowResponse>>]
    [HttpGet("circles")]
    public async Task<IActionResult> GetPaginatedCircles([FromQuery] BaseQueryStringWithQ query, CancellationToken ct)
    {
        try
        {
            var response = await circleService.GetAsync(query, ct);

            return Ok(_response.SuccessResponse(response));
        }
        catch (Exception e)
        {
            return NotFound(_response.FailureResponse(e.Message));
        }
    }
}
