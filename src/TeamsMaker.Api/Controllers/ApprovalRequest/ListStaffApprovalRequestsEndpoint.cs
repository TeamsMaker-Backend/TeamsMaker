using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Contracts.QueryStringParameters;
using TeamsMaker.Api.Contracts.Responses.ApprovalRequest;
using TeamsMaker.Api.Services.ApprovalRequests.Interfaces;

namespace TeamsMaker.Api.Controllers.ApprovalRequest;

[Authorize]
public class ListStaffApprovalRequestsEndpoint(IApprovalRequestService approvalRequestService) : BaseApiController
{
    [Tags("proposals/approval_requests")]
    //[Produces<List<GetStaffApprovalRequestResponse>>]
    [HttpGet("proposals/approval_requests")]
    public async Task<IActionResult> StaffApprovalRequests([FromQuery] ApprovalRequestQueryString queryString, CancellationToken ct)
    {
        List<GetStaffApprovalRequestResponse> approvalRequestResponse;

        try
        {
            approvalRequestResponse = await approvalRequestService.ListStaffAsync(queryString, ct);
        }
        catch (Exception e)
        {
            return NotFound(_response.FailureResponse(e.Message));
        }

        return Ok(_response.SuccessResponse(approvalRequestResponse));
    }
}
