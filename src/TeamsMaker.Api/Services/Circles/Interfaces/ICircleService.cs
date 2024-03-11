using TeamsMaker.Api.Contracts.Requests.Circle;

namespace TeamsMaker.Api.Services.Circles.Interfaces;

public interface ICircleService
{
    Task UpdateLinksAsync(Guid id, UpdateCircleLinksRequest request, CancellationToken ct);
}
