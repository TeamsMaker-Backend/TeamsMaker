using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Contracts.Requests.Session;
using TeamsMaker.Api.Services.Circles.Interfaces;

namespace TeamsMaker.Api.Controllers.Circles.Sessions;

[Authorize]
public class UpdateSessionInfoEndpoint(ISessionService sessionService) : BaseApiController
{
    [Tags("circles/sessions")]
    [HttpPut("circles/sessions/{id}")]
    public async Task<IActionResult> Session(Guid id, UpdateSessionInfoRequest request, CancellationToken ct)
    {
        try
        {
            await sessionService.UpdateInfoAsync(id, request, ct);
        }
        catch (ArgumentException e)
        {
            return NotFound(_response.FailureResponse(e.Message));
        }

        return Ok(_response.SuccessResponse(null));
    }
}
