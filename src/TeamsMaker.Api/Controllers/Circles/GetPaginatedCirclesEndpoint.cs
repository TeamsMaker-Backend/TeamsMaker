using System.ComponentModel;

using Core.Generics;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Swashbuckle.AspNetCore.Annotations;

using TeamsMaker.Api.Contracts.QueryStringParameters;

using TeamsMaker.Api.Contracts.Responses.Circle;
using TeamsMaker.Api.Services.Circles.Interfaces;

namespace TeamsMaker.Api.Controllers.Circles;

[Authorize]
public class GetPaginatedCirclesEndpoint(ICircleService circleService) : BaseApiController
{
    [Tags("circles")]
    [Produces<PagedList<GetCircleAsRowResponse>>]
    [SwaggerOperation(Summary = "get circles with pagination, also search by circle name or user name to get his circle")]
    [HttpGet("circles")]
    public async Task<IActionResult> GetPaginatedCircles([FromQuery] BaseQueryStringWithQ query, CancellationToken ct)
    {
        try
        {
            var response = await circleService.GetAsync(query, ct);

            return Ok(_response.SuccessResponseWithPagination(response));
        }
        catch (Exception e)
        {
            return NotFound(_response.FailureResponse(e.Message));
        }
    }
}
