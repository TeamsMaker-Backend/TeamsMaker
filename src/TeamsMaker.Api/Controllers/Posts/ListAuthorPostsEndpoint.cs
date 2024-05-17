using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Contracts.QueryStringParameters;
using TeamsMaker.Api.Contracts.Responses;
using TeamsMaker.Api.Contracts.Responses.Post;
using TeamsMaker.Api.DataAccess.Models;
using TeamsMaker.Api.Services.Posts.Interfaces;

namespace TeamsMaker.Api.Controllers.Posts;

[Authorize]
public class ListAuthorPostsEndpoint(IPostService postService) : BaseApiController
{
    [Tags("posts")]
    [HttpGet("posts/authors/{entityId}")]
    public async Task<IActionResult> ListAuthorPosts(string entityId,[FromQuery] PostsQueryString postsQueryString, CancellationToken ct)
    {
        try
        {
            var posts = await postService.ListAuthorPostsAsync(entityId, postsQueryString, ct);
            return posts is not null ? Ok(_response.SuccessResponseWithPagination(posts)) : NotFound();
        }
        catch (Exception e)
        {
            return NotFound(_response.FailureResponse(e.Message));
        }
    }
}
