using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Contracts.Requests.JoinRequest;
using TeamsMaker.Api.Services.JoinRequests.Interfaces;

namespace TeamsMaker.Api.Controllers.JoinRequests;

[Authorize]
public class AddJoinRequestEndPoint(IJoinRequestService joinRequestService) : BaseApiController
{
    [Tags("join Request")]
    [HttpPost("join_request")]
    public async Task<IActionResult> JoinRequest(AddJoinRequest request, CancellationToken ct)
    {

        try
        {
            await joinRequestService.AddJoinRequestAsync(request, ct);
        }
        catch (ArgumentException e)
        {
            return NotFound(_response.FailureResponse(e.Message));
        }

        return Ok();
    }

}
