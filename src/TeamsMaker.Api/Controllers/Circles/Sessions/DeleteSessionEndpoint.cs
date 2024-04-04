using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Services.Circles.Interfaces;

namespace TeamsMaker.Api.Controllers.Circles.Sessions;

[Authorize]
public class DeleteSessionEndpoint(ISessionService sessionService) : BaseApiController
{
    [Tags("circles/sessions")]
    [HttpDelete("circles/sessions/{id}")]
    public async Task<IActionResult> Session(Guid id, CancellationToken ct)
    {
        try
        {
            await sessionService.DeleteAsync(id, ct);
        }
        catch (ArgumentException e)
        {
            return NotFound(_response.FailureResponse(e.Message));
        }

        return Ok(_response.SuccessResponse(null));
    }
}
