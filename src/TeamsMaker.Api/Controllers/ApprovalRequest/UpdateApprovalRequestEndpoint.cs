using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Services.ApprovalRequests.Interfaces;

namespace TeamsMaker.Api.Controllers.ApprovalRequest;

[Authorize]
public class UpdateApprovalRequestEndpoint(IApprovalRequestService approvalRequestService) : BaseApiController
{
    [Tags("proposals/approval_requests")]
    [HttpPut("proposals/approval_requests/{id}")]
    public async Task<IActionResult> ApprovalRequest(Guid id, bool isAccepted, CancellationToken ct)
    {
        try
        {
            await approvalRequestService.UpdateAsync(id, isAccepted, ct);
        }
        catch (Exception e)
        {
            return NotFound(_response.FailureResponse(e.Message));
        }

        return Ok();
    }
}
