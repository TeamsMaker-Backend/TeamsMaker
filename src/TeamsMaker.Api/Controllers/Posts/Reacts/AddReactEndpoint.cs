using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Contracts.Responses;
using TeamsMaker.Api.Services.Posts.Interfaces;

namespace TeamsMaker.Api.Controllers.Posts.Reacts;

[Authorize]
public class AddReactEndpoint(IPostService postService) : BaseApiController
{
    [Tags("posts/reacts")]
    [Produces<IdResponse<Guid>>]
    [HttpPost("posts/{postId}/reacts")]
    public async Task<IActionResult> AddReact(Guid postId, CancellationToken ct)
    {
        Guid reactId;
        try
        {
            reactId = await postService.AddReactAsync(postId, ct);
        }
        catch (Exception e)
        {
            return NotFound(_response.FailureResponse(e.Message));
        }
        return Created("", _response.SuccessResponse(new IdResponse<Guid>(reactId)));
    }
}
