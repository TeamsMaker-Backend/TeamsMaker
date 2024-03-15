using TeamsMaker.Api.Contracts.Requests.Join_Request;

namespace TeamsMaker.Api.Services.Join_Requests.Interfaces
{
    public interface IJoinRequestService
    {
        Task AddJoinRequestAsync(AddJoinRequest request, CancellationToken ct);
    }
}
