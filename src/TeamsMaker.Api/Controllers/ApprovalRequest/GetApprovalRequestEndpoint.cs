using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Contracts.Responses.ApprovalRequest;
using TeamsMaker.Api.Services.ApprovalRequests.Interfaces;

namespace TeamsMaker.Api.Controllers.ApprovalRequest;

[Authorize]
public class GetApprovalRequestEndpoint(IApprovalRequestService approvalRequestService) : BaseApiController
{
    [Tags("proposals/approval_requests")]
    [Produces<GetApprovalRequestResponse>]
    [HttpGet("proposals/approval_requests/{id}")]
    public async Task<IActionResult> ApprovalRequests(Guid id, CancellationToken ct)
    {
        GetApprovalRequestResponse approvalRequestResponse;

        try
        {
            approvalRequestResponse = await approvalRequestService.GetAsync(id, ct);
        }
        catch (Exception e)
        {
            return NotFound(_response.FailureResponse(e.Message));
        }

        return Ok(_response.SuccessResponse(approvalRequestResponse));
    }
}
