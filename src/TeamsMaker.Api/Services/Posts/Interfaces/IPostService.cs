using TeamsMaker.Api.Contracts.Requests.Post;

namespace TeamsMaker.Api.Services.Posts.Interfaces;

public interface IPostService
{
    Task<Guid> AddAsync(AddPostRequest request, CancellationToken ct);
}