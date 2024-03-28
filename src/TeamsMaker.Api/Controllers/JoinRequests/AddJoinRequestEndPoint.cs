using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Contracts.Requests.JoinRequest;
using TeamsMaker.Api.Services.JoinRequests.Interfaces;

namespace TeamsMaker.Api.Controllers.JoinRequests;

[Authorize]
public class AddJoinRequestEndpoint(IJoinRequestService joinRequestService) : BaseApiController
{
    [Tags("join_request")]
    [HttpPost("join_request")]
    public async Task<IActionResult> JoinRequest(AddJoinRequest request, CancellationToken ct)
    {
        Guid joinRequestId;

        try
        {
            joinRequestId = await joinRequestService.AddAsync(request, ct);
        }
        catch (ArgumentException e)
        {
            return NotFound(_response.FailureResponse(e.Message));
        }

        return Created("", _response.SuccessResponse(new { joinRequestId }));
    }

}
