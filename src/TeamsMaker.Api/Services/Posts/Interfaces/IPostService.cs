using Core.Generics;

using TeamsMaker.Api.Contracts.QueryStringParameters;
using TeamsMaker.Api.Contracts.Requests.Post;
using TeamsMaker.Api.Contracts.Responses.Post;

namespace TeamsMaker.Api.Services.Posts.Interfaces;

public interface IPostService
{
    Task<Guid> AddAsync(AddPostRequest request, CancellationToken ct);
    Task<GetPostResponse> GetPostAsync(Guid id, CancellationToken ct);
    Task<PagedList<GetPostResponse>> ListAuthorPostsAsync(string id, PostsQueryString postsQueryString, CancellationToken ct);
    Task<PagedList<GetPostResponse>> ListFeedsPostsAsync(string id, PostsQueryString postsQueryString, CancellationToken ct);
}