using TeamsMaker.Api.Contracts.QueryStringParameters;
using TeamsMaker.Api.Contracts.Requests.ApprovalRequest;
using TeamsMaker.Api.Contracts.Responses.ApprovalRequest;

namespace TeamsMaker.Api.Services.ApprovalRequests.Interfaces;

public interface IApprovalRequestService
{
    Task<Guid> AddAsync(AddApprovalRequest request, CancellationToken ct);
    Task UpdateAsync(Guid id, bool isAccepted, CancellationToken ct);
    Task CancelAsync(Guid id, CancellationToken ct);
    Task<ListCircleApprovalRequestResponse> ListCircleAsync(Guid proposalId, ApprovalRequestQueryString queryString, CancellationToken ct);
    Task<ListStaffApprovalRequestResponse> ListStaffAsync(ApprovalRequestQueryString queryString, CancellationToken ct);
}
