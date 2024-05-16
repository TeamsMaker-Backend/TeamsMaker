using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Contracts.Requests.ApprovalRequest;
using TeamsMaker.Api.Contracts.Responses;
using TeamsMaker.Api.Services.ApprovalRequests.Interfaces;

namespace TeamsMaker.Api.Controllers.ApprovalRequest;

[Authorize]
public class AddApprovalRequestEndpoint(IApprovalRequestService approvalRequestService) : BaseApiController
{
    [Tags("proposals/approval_requests")]
    [Produces<IdResponse<Guid>>]
    [HttpPost("proposals/approval_request")]
    public async Task<IActionResult> ApprovalRequest(AddApprovalRequest request, CancellationToken ct)
    {
        Guid approvalRequestId;

        try
        {
            approvalRequestId = await approvalRequestService.AddAsync(request, ct);
        }
        catch (Exception e)
        {
            return NotFound(_response.FailureResponse(e.Message));
        }

        return Created("", _response.SuccessResponse(new IdResponse<Guid>(approvalRequestId)));
    }
}
