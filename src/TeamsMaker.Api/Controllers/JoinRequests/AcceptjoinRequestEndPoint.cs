using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Services.JoinRequests.Interfaces;

namespace TeamsMaker.Api.Controllers.JoinRequests
{
    [Authorize]
    public class AcceptjoinRequestEndPoint(IJoinRequestService joinRequestService) : BaseApiController
    {
        [Tags("join Request")]
        [HttpPut("join_request/{id}")]
        public async Task<IActionResult> JoinRequest(Guid id, CancellationToken ct)
        {

            try
            {
                await joinRequestService.AcceptAsync(id, ct);
            }
            catch (ArgumentException e)
            {
                return NotFound(_response.FailureResponse(e.Message));
            }

            return Ok();
        }
    }
}
