using TeamsMaker.Api.Contracts.Requests.JoinRequest;
using TeamsMaker.Api.Contracts.Responses.JoinRequest;

namespace TeamsMaker.Api.Services.JoinRequests.Interfaces;

public interface IJoinRequestService
{
    Task<Guid> AddAsync(AddJoinRequest request, CancellationToken ct);
    Task AddAsync(List<AddJoinRequest> requests, CancellationToken ct);
    Task<GetJoinRequestResponse> GetAsync(string? circleId, CancellationToken ct);
    Task AcceptAsync(Guid id, CancellationToken ct);
    Task DeleteAsync(Guid id, CancellationToken ct);
}
