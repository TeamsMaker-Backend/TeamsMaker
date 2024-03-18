using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Contracts.Requests.Session;
using TeamsMaker.Api.Services.Circles.Interfaces;

namespace TeamsMaker.Api.Controllers.Circles.Sessions;

[Authorize]
public class AddSessionEndpoint(ISessionService sessionService) : BaseApiController
{
    [Tags("circles/sessions")]
    [HttpPost("circles/sessions")]
    public async Task<IActionResult> Session(AddSessionRequest request, CancellationToken ct)
    {
        Guid sessionId;

        try
        {
            sessionId = await sessionService.AddAsync(request, ct);
        }
        catch (ArgumentException e)
        {
            return NotFound(_response.FailureResponse(e.Message));
        }

        return Created("", _response.SuccessResponse(new { sessionId }));
    }
}
