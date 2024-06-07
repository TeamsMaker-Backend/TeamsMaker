using Core.Generics;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Swashbuckle.AspNetCore.Annotations;

using TeamsMaker.Api.Contracts.QueryStringParameters;
using TeamsMaker.Api.Contracts.Responses.Circle;
using TeamsMaker.Api.Services.Circles.Interfaces;

namespace TeamsMaker.Api.Controllers.Circles;

[Authorize]
public class GetArchiveEndpoint(ICircleService circleService) : BaseApiController
{
    [Tags("circles")]
    [Produces<PagedList<GetCircleAsCardResponse>>]
    [SwaggerOperation(Summary = "get archieved circles")]
    [HttpGet("circles/archive")]
    public async Task<IActionResult> ArchievedCircles(ArchiveQueryString archiveQuery, CancellationToken ct)
    {
        try
        {
            var archievedCircles = await circleService.GetArchiveAsync(archiveQuery, ct);
            return archievedCircles is not null ? Ok(_response.SuccessResponseWithPagination(archievedCircles)) : NotFound();
        }
        catch (Exception e)
        {
            return NotFound(_response.FailureResponse(e.Message));
        }
    }
}
