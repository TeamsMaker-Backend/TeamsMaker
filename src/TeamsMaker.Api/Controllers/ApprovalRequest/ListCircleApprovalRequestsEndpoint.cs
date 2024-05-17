using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Contracts.QueryStringParameters;
using TeamsMaker.Api.Contracts.Responses.ApprovalRequest;
using TeamsMaker.Api.Services.ApprovalRequests.Interfaces;

namespace TeamsMaker.Api.Controllers.ApprovalRequest;

[Authorize]
public class ListCircleApprovalRequestsEndpoint(IApprovalRequestService approvalRequestService) : BaseApiController
{
    [Tags("proposals/approval_requests")]
    //[Produces<List<GetCircleApprovalRequestResponse>>]
    [HttpGet("proposals/{id}/approval_requests")]
    public async Task<IActionResult> CircleApprovalRequests(Guid id, [FromQuery] ApprovalRequestQueryString queryString, CancellationToken ct)
    {
        List<GetCircleApprovalRequestResponse> approvalRequestResponse;

        try
        {
            approvalRequestResponse = await approvalRequestService.ListCircleAsync(id, queryString, ct);
        }
        catch (Exception e)
        {
            return NotFound(_response.FailureResponse(e.Message));
        }

        return Ok(_response.SuccessResponse(approvalRequestResponse));
    }
}
