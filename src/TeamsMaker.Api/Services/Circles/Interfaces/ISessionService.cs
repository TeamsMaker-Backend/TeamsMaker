using TeamsMaker.Api.Contracts.Requests.Session;

namespace TeamsMaker.Api.Services.Circles.Interfaces;

public interface ISessionService
{
    Task<Guid> AddAsync(AddSessionRequest request, CancellationToken ct);
    Task UpdateInfoAsync(Guid id, UpdateSessionInfoRequest request, CancellationToken ct);
    Task UpdateStatusAsync(Guid id, SessionStatus status, CancellationToken ct);
    Task DeleteAsync(Guid id, CancellationToken ct);
}
