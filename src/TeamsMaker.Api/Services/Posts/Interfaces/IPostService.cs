using Core.Generics;

using TeamsMaker.Api.Contracts.QueryStringParameters;
using TeamsMaker.Api.Contracts.Requests.Post;
using TeamsMaker.Api.Contracts.Responses.Post;

namespace TeamsMaker.Api.Services.Posts.Interfaces;

public interface IPostService
{
    Task<Guid> AddAsync(AddPostRequest request, CancellationToken ct);
    Task DeleteAsync(Guid id, CancellationToken ct, bool isAuthorized = false);
    Task UpdateAsync(Guid id, UpdatePostRequest request, CancellationToken ct);
    Task<Guid> AddReactAsync(Guid postId, CancellationToken ct);
    Task DeleteReactAsync(Guid postId, CancellationToken ct);
    Task<GetPostResponse> GetPostAsync(Guid id, CancellationToken ct);
    Task<PagedList<GetPostResponse>> ListAuthorPostsAsync(string id, PostsQueryString postsQueryString, CancellationToken ct);
    Task<PagedList<GetPostResponse>> ListFeedPostsAsync(bool isCircle, PostsQueryString postsQueryString, CancellationToken ct);
}