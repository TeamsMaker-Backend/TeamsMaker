using TeamsMaker.Api.Contracts.Requests.Post;

namespace TeamsMaker.Api.Services.Reacts.Interfaces;

public interface IReactService
{
    Task<Guid> AddAsync(Guid id, CancellationToken ct);
    Task DeleteAsync(Guid id, CancellationToken ct);

}
