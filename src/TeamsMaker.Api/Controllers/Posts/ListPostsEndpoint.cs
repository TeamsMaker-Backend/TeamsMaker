using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Contracts.QueryStringParameters;
using TeamsMaker.Api.Services.Posts.Interfaces;

namespace TeamsMaker.Api.Controllers.Posts;

public class ListPostsEndpoint(IPostService postService): BaseApiController
{
    //[Tags("posts")]
    //[HttpGet("posts/feeds/{id}")]
    //public async Task<IActionResult> ListfeddsPosts(string id, [FromQuery] PostsQueryString postsQueryString, CancellationToken ct)
    //{
    //    try
    //    {
    //        var posts = await postService.ListFeedsPostsAsync(id, postsQueryString, ct);
    //        return posts is not null ? Ok(_response.SuccessResponseWithPagination(posts)) : NotFound();
    //    }
    //    catch (Exception e)
    //    {
    //        return NotFound(_response.FailureResponse(e.Message));
    //    }
    //}
}
