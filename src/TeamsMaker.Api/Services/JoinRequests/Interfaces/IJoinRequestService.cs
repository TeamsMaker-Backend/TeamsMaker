using TeamsMaker.Api.Contracts.Requests.JoinRequest;
using TeamsMaker.Api.Contracts.Responses.JoinRequest;

namespace TeamsMaker.Api.Services.JoinRequests.Interfaces
{
    public interface IJoinRequestService
    {
        Task AddJoinRequestAsync(AddJoinRequest request, CancellationToken ct);
        Task<List<GetCircleJoinRequestResponse>> GetCircleJoinRequesAsync(string id, CancellationToken ct);
        Task AcceptJoinRequestAsync(Guid id, CancellationToken ct);
        Task DeleteJoinRequestAsync(Guid id, CancellationToken ct);
    }
}
