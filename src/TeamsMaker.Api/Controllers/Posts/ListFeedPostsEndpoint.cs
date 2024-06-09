using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Contracts.QueryStringParameters;
using TeamsMaker.Api.Services.Posts.Interfaces;

namespace TeamsMaker.Api.Controllers.Posts;

[Authorize]
public class ListFeedPostsEndpoint(IPostService postService) : BaseApiController
{
    [Tags("posts")]
    [HttpGet("posts/feed/{isCircle}")]
    public async Task<IActionResult> ListfeddsPosts(bool isCircle, [FromQuery] PostsQueryString postsQueryString, CancellationToken ct)
    {
        try
        {
            var posts = await postService.ListFeedPostsAsync(isCircle, postsQueryString, ct);
            return posts is null ? NotFound() : Ok(_response.SuccessResponseWithPagination(posts));
        }
        catch (Exception e)
        {
            return NotFound(_response.FailureResponse(e.Message));
        }
    }
}
