using TeamsMaker.Api.Contracts.Requests.Post;

namespace TeamsMaker.Api.Services.Posts.Interfaces;

public interface IPostService
{
    Task<Guid> AddAsync(AddPostRequest request, CancellationToken ct);
    Task DeleteAsync(Guid id, CancellationToken ct, bool isAuthorized = false);
    Task UpdateAsync(Guid id, UpdatePostRequest request, CancellationToken ct);
    Task<Guid> AddReactAsync(Guid postId, CancellationToken ct);
    Task DeleteReactAsync(Guid postId, CancellationToken ct);
}