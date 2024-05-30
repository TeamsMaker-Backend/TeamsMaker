using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Services.Posts.Interfaces;

namespace TeamsMaker.Api.Controllers.Posts.Reacts;

[Authorize]
public class DeleteReactEndpoint(IPostService postService) : BaseApiController
{
    [Tags("posts/reacts")]
    [HttpDelete("posts/{postId}/reacts")]
    public async Task<IActionResult> DeleteReact(Guid postId, CancellationToken ct)
    {
        try
        {
            await postService.DeleteReactAsync(postId, ct);
        }
        catch (Exception e)
        {
            return NotFound(_response.FailureResponse(e.Message));
        }
        return Ok(_response.SuccessResponse(null));
    }
}
