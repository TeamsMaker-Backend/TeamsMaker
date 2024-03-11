
using TeamsMaker.Api.Contracts.Requests.NewFolder;

namespace TeamsMaker.Api.Services.Circles.Interfaces;

public interface ICircleService
{
    Task AddCircleAsync(AddCircleRequest request, CancellationToken ct);
}
