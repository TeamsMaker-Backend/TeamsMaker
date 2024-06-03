using Core.Generics;

using TeamsMaker.Api.Contracts.QueryStringParameters;

using TeamsMaker.Api.Contracts.Requests.Circle;
using TeamsMaker.Api.Contracts.Responses.Circle;

namespace TeamsMaker.Api.Services.Circles.Interfaces;

public interface ICircleService
{
    Task<Guid> AddAsync(AddCircleRequest request, CancellationToken ct);
    Task<GetCircleResponse> GetAsync(Guid circleId, CancellationToken ct);
    Task<PagedList<GetCircleAsRowResponse>> GetAsync(BaseQueryStringWithQ query, CancellationToken ct);
    Task<List<GetCircleAsRowResponse>> GetAsync(CancellationToken ct);
    Task<PagedList<GetCircleAsCardResponse>> GetArchiveAsync(ArchiveQueryString archiveQuery, CancellationToken ct);
    Task<GetCircleMembersResponse> GetMembersAsync(Guid circleId, CancellationToken ct);
    Task UpdateInfoAsync(Guid circleId, UpdateCircleInfoRequest request, CancellationToken ct);
    Task UpdatePrivacyAsync(Guid circleId, bool isPublic, CancellationToken ct);
    Task UpvoteAsync(Guid circleId, CancellationToken ct);
    Task DownvoteAsync(Guid id, CancellationToken ct);
    Task TransferOwnershipAsync(Guid circleId, string newOwnerId, CancellationToken ct);
    Task ArchiveAsync(Guid circleId, CancellationToken ct);
    Task DeleteAsync(Guid circleId, CancellationToken ct);
}
