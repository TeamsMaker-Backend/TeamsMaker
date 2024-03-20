using TeamsMaker.Api.Contracts.Requests.JoinRequest;
using TeamsMaker.Api.Contracts.Responses.JoinRequest;

namespace TeamsMaker.Api.Services.JoinRequests.Interfaces;

public interface IJoinRequestService
{
    Task AddAsync(AddJoinRequest request, CancellationToken ct);
    Task<List<GetCircleJoinRequestResponse>> GetAsync(string id, CancellationToken ct);
    Task AcceptAsync(Guid id, CancellationToken ct);
    Task DeleteAsync(Guid id, CancellationToken ct);
}
