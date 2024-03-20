using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Contracts.Responses.JoinRequest;
using TeamsMaker.Api.Services.JoinRequests.Interfaces;

namespace TeamsMaker.Api.Controllers.JoinRequests
{
    [Authorize]
    public class GetCircleEndpoint(IJoinRequestService joinRequestService) : BaseApiController
    {
        [Tags("join_request")]
        [HttpGet("join_request/{id}")]
        public async Task<IActionResult> JoinRequest(string id, CancellationToken ct)
        {
            List<GetCircleJoinRequestResponse> response;

            try
            {
                response = await joinRequestService.GetAsync(id, ct);
            }
            catch (ArgumentException e)
            {
                return NotFound(_response.FailureResponse(e.Message));
            }

            return Ok(_response.SuccessResponse(response));
        }
    }

}
