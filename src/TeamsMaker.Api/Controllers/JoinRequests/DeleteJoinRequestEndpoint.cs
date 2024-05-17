using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Services.JoinRequests.Interfaces;

namespace TeamsMaker.Api.Controllers.JoinRequests;

[Authorize]
public class DeleteJoinRequestEndpoint(IJoinRequestService joinRequestService) : BaseApiController
{
    [Tags("join_request")]
    [HttpDelete("join_request/{id}")]
    public async Task<IActionResult> JoinRequest(Guid id, [FromQuery] Guid? circleId, CancellationToken ct)
    {
        try
        {
            await joinRequestService.DeleteAsync(id, circleId, ct);
        }
        catch (Exception e)
        {
            return NotFound(_response.FailureResponse(e.Message));
        }

        return Ok(_response.SuccessResponse(null));
    }
}
