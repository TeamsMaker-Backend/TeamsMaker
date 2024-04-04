using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Contracts.Responses.Circle;
using TeamsMaker.Api.Services.Circles.Interfaces;

namespace TeamsMaker.Api.Controllers.Circles.CircleMembers;

[Authorize]
public class GetCircleMembersEndpoint(ICircleService circleService) : BaseApiController
{
    [Tags("circles/members")]
    [Produces(typeof(GetCircleMembersResponse))]
    [HttpGet("circles/{circleId}/members")]
    public async Task<IActionResult> CircleMembers(Guid circleId, CancellationToken ct)
    {
        GetCircleMembersResponse response;

        try
        {
            response = await circleService.GetMembersAsync(circleId, ct);
        }
        catch (ArgumentException e)
        {
            return NotFound(_response.FailureResponse(e.Message));
        }

        return Ok(_response.SuccessResponse(response));
    }
}
