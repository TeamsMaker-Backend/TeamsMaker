using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Contracts.Requests.Post;
using TeamsMaker.Api.Contracts.Responses;
using TeamsMaker.Api.Contracts.Responses.Post;
using TeamsMaker.Api.Services.Posts.Interfaces;

namespace TeamsMaker.Api.Controllers.Posts;

[Authorize]
public class GetPostEndpoint(IPostService postService) : BaseApiController
{
    [Tags("posts")]
    [HttpGet("posts/{id}")]
    public async Task<IActionResult> GetPost(Guid id, CancellationToken ct)
    {
        GetPostResponse response;
        try
        {
           response =  await postService.GetPostAsync(id, ct);
        }
        catch (Exception e)
        {
            return NotFound(_response.FailureResponse(e.Message));
        }
        return Ok(_response.SuccessResponse(response));
    }
}
