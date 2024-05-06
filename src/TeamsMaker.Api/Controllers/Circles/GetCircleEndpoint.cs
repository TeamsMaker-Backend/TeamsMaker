using System.ComponentModel;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Swashbuckle.AspNetCore.Annotations;

using TeamsMaker.Api.Contracts.Responses.Circle;
using TeamsMaker.Api.Services.Circles.Interfaces;

namespace TeamsMaker.Api.Controllers.Circles;

[Authorize]
public class GetCircleEndpoint(ICircleService circleService) : BaseApiController
{
    [Tags("circles")]
    [Produces<GetCircleResponse>]
    [SwaggerOperation(Summary = "get circle page")]
    [HttpGet("circles/{circleId}")]
    public async Task<IActionResult> Circle(Guid circleId, CancellationToken ct)
    {
        GetCircleResponse response;

        try
        {
            response = await circleService.GetAsync(circleId, ct);
        }
        catch (Exception e)
        {
            return NotFound(_response.FailureResponse(e.Message));
        }

        return Ok(_response.SuccessResponse(response));
    }
}
