using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Contracts.Requests.Circle;

using TeamsMaker.Api.Services.Circles.Interfaces;

namespace TeamsMaker.Api.Controllers.Circles.CircleMembers;

[Authorize]
public class UpdateCircleMemberEndpoint(ICircleMemberService memberService) : BaseApiController
{
    [Tags("circles/members")]
    [HttpPatch("members/{memberId}")]
    public async Task<IActionResult> CircleMember(Guid memberId, [FromBody] UpdateCircleMemberRequest request, CancellationToken ct)
    {
        try
        {
            await memberService.RemoveAsync(memberId, ct);
        }
        catch (Exception e)
        {
            return NotFound(_response.FailureResponse(e.Message));
        }

        return Ok(_response.SuccessResponse(null));
    }
}