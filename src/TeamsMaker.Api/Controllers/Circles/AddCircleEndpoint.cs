using Azure.Core;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Contracts.Requests.NewFolder;
using TeamsMaker.Api.Services.Circles.Interfaces;

namespace TeamsMaker.Api.Controllers.Circles;

[Authorize]
public class AddCircleEndpoint(ICircleService circleService) : BaseApiController
{
    private readonly ICircleService _circleService = circleService;

    [Tags("Circles")]
    [HttpPost("Circles")]
    public async Task<IActionResult> Circle(AddCircleRequest request, CancellationToken ct)
    {
        try
        {
            await _circleService.AddCircleAsync(request, ct);
        }
        catch (ArgumentException e)
        {
            return NotFound(_response.FailureResponse(e.Message));
        }
        return Ok();
    }
}
