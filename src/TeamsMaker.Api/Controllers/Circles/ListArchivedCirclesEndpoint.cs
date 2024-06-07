using Core.Generics;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Swashbuckle.AspNetCore.Annotations;

using TeamsMaker.Api.Contracts.QueryStringParameters;
using TeamsMaker.Api.Contracts.Responses.Circle;
using TeamsMaker.Api.Services.Circles.Interfaces;

namespace TeamsMaker.Api.Controllers.Circles;

[Authorize]
public class ListArchivedCirclesEndpoint(ICircleService circleService) : BaseApiController
{
    [Tags("circles")]
    [Produces<PagedList<GetCircleAsCardResponse>>]
    [SwaggerOperation(Summary = "get archived circles with pagination, also search by circle name or user name to get his circle")]
    [HttpGet("circles/archived")]
    public async Task<IActionResult> ArchievedCircles([FromQuery] ArchiveQueryString archiveQuery, CancellationToken ct)
    {
        try
        {
            var archievedCircles = await circleService.ListArchivedAsync(archiveQuery, ct);
            return archievedCircles is not null ? Ok(_response.SuccessResponseWithPagination(archievedCircles)) : NotFound();
        }
        catch (Exception e)
        {
            return NotFound(_response.FailureResponse(e.Message));
        }
    }
}
