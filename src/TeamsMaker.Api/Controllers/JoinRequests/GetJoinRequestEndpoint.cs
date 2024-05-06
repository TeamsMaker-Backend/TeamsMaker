using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Swashbuckle.AspNetCore.Annotations;

using TeamsMaker.Api.Contracts.Responses.JoinRequest;
using TeamsMaker.Api.Services.JoinRequests.Interfaces;

namespace TeamsMaker.Api.Controllers.JoinRequests;

[Authorize]
public class GetJoinRequestEndpoint(IJoinRequestService joinRequestService) : BaseApiController
{
    [Tags("join_request")]
    [Produces<GetJoinRequestResponse>]
    [SwaggerOperation(Description = "If circleId has value get circle join requests, if not userId will be used to get user join requests")]
    [HttpGet("join_requests")]
    public async Task<IActionResult> JoinRequest([FromQuery] string? circleId, CancellationToken ct)
    {
        GetJoinRequestResponse response;

        try
        {
            response = await joinRequestService.GetAsync(circleId, ct);
        }
        catch (Exception e)
        {
            return NotFound(_response.FailureResponse(e.Message));
        }

        return Ok(_response.SuccessResponse(response));
    }
}
