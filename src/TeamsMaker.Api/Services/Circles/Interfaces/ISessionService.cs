using TeamsMaker.Api.Contracts.Requests.Session;

namespace TeamsMaker.Api.Services.Circles.Interfaces;

public interface ISessionService
{
    Task AddAsync(AddSessionRequest request, CancellationToken ct);
}
