using TeamsMaker.Api.Contracts.Requests.Circle;

namespace TeamsMaker.Api.Services.Circles.Interfaces;

public interface ICircleService
{
    Task UpdateInfoAsync(Guid id, UpdateCircleInfoRequest request, CancellationToken ct);
    Task UpdateLinksAsync(Guid id, UpdateCircleLinksRequest request, CancellationToken ct);
    Task UpdatePrivacyAsync(Guid id, bool isPublic, CancellationToken ct);
}
