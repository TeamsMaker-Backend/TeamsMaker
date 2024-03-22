using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Services.Circles.Interfaces;

namespace TeamsMaker.Api.Controllers.Circles.CircleMembers;

[Authorize]
public class RemoveCircleMemberEndpoint(ICircleMemberService memberService) : BaseApiController
{
    [Tags("circles/circle_members")]
    [HttpDelete("circle_members/{memberID}")]
    public async Task<IActionResult> CircleMember(Guid memberId, CancellationToken ct)
    {
        try
        {
            await memberService.RemoveAsync(memberId, ct);
        }
        catch (ArgumentException e)
        {
            return NotFound(_response.FailureResponse(e.Message));
        }

        return Ok();
    }
}
