using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Contracts.Responses.JoinRequest;
using TeamsMaker.Api.Services.JoinRequests.Interfaces;

namespace TeamsMaker.Api.Controllers.JoinRequests;

[Authorize]
public class GetJoinRequestEndpoint(IJoinRequestService joinRequestService) : BaseApiController
{
    [Tags("join_request")]
    [Produces<GetJoinRequestResponse>]
    [HttpGet("join_requests")]
    public async Task<IActionResult> JoinRequest([FromQuery] string? circleId, CancellationToken ct)
    {
        GetJoinRequestResponse response;

        try
        {
            response = await joinRequestService.GetAsync(circleId, ct);
        }
        catch (ArgumentException e)
        {
            return NotFound(_response.FailureResponse(e.Message));
        }

        return Ok(_response.SuccessResponse(response));
    }
}
