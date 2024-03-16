using TeamsMaker.Api.Contracts.Requests.JoinRequest;

namespace TeamsMaker.Api.Services.JoinRequests.Interfaces
{
    public interface IJoinRequestService
    {
        Task AddJoinRequestAsync(AddJoinRequest request, CancellationToken ct);
        Task DeleteJoinRequestAsync(Guid id, CancellationToken ct);
        //Task AcceptJoinRequestAsync(CancellationToken ct);
    }
}
