using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Contracts.Requests.Circle;
using TeamsMaker.Api.Services.Circles.Interfaces;

namespace TeamsMaker.Api.Controllers.Circles;

[Authorize]
public class AddCircleEndpoint(ICircleService circleService) : BaseApiController
{
    [Tags("circles")]
    [HttpPost("circles")]
    public async Task<IActionResult> Circle(AddCircleRequest request, CancellationToken ct)
    {
        Guid circleId;

        try
        {
            circleId = await circleService.AddAsync(request, ct);
        }
        catch (ArgumentException e)
        {
            return NotFound(_response.FailureResponse(e.Message));
        }

        return Created("", _response.SuccessResponse(new { circleId }));
    }
}
