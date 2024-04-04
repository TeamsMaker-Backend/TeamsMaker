using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Services.Circles.Interfaces;

namespace TeamsMaker.Api.Controllers.Circles.Sessions;

[Authorize]
public class UpdateSessionStatuseEndpoint(ISessionService sessionService) : BaseApiController
{
    [Tags("circles/sessions")]
    [HttpPut("circles/sessions/{id}/{status}")] //patch
    public async Task<IActionResult> Session(Guid id, SessionStatus status, CancellationToken ct)
    {
        try
        {
            await sessionService.UpdateStatusAsync(id, status, ct);
        }
        catch (ArgumentException e)
        {
            return NotFound(_response.FailureResponse(e.Message));
        }

        return Ok(_response.SuccessResponse(null));
    }
}
