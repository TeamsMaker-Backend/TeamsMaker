using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Services.Circles.Interfaces;

namespace TeamsMaker.Api.Controllers.Circles.CircleMembers;

[Authorize]
public class AddCircleMemberEndpoint(ICircleMemberService memberService) : BaseApiController
{
    [Tags("circles/circle_members")]
    [HttpPost("circles/{circleId}/circle_members/{userID}")]
    public async Task<IActionResult> CircleMember(Guid circleId, string userId, CancellationToken ct)
    {
        try
        {
            await memberService.AddAsync(circleId, userId, ct);
        }
        catch (ArgumentException e)
        {
            return NotFound(_response.FailureResponse(e.Message));
        }

        return Ok();
    }
}
