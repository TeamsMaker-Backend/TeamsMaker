using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Services.Circles.Interfaces;

namespace TeamsMaker.Api.Controllers.Circles.CircleMembers;

[Authorize]
public class UpdateCircleMemberBadgeEndpoint(ICircleMemberService memberService) : BaseApiController
{
    [Tags("circles/members")]
    [HttpPut("members/{memberId}/badge/{badge}")]
    public async Task<IActionResult> CircleMemberBadge(Guid memberId, string? badge, CancellationToken ct)
    {
        try
        {
            await memberService.UpdateBadgeAsync(memberId, badge, ct);
        }
        catch (ArgumentException e)
        {
            return NotFound(_response.FailureResponse(e.Message));
        }

        return Ok(_response.SuccessResponse(null));
    }
}
