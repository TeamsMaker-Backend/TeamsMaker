using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Services.ApprovalRequests.Interfaces;

namespace TeamsMaker.Api.Controllers.ApprovalRequest;

[Authorize]
public class CancelApprovalRequestEndpoint(IApprovalRequestService approvalRequestService) : BaseApiController
{
    [Tags("proposals/approval_requests")]
    [HttpDelete("proposals/approval_requests/{id}")]
    public async Task<IActionResult> ApprovalRequest(Guid id, CancellationToken ct)
    {
        try
        {
            await approvalRequestService.CancelAsync(id, ct);
        }
        catch (Exception e)
        {
            return NotFound(_response.FailureResponse(e.Message));
        }

        return Ok();
    }
}
