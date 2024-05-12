using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Services.Posts.Interfaces;

namespace TeamsMaker.Api.Controllers.Posts;

public class DeletePostEndpoint(IPostService postService) : BaseApiController
{
    [Tags("posts")]
    [HttpDelete("posts/{id}")]
    public async Task<IActionResult> DeletePost(Guid id, CancellationToken ct)
    {
        try
        {
            await postService.DeleteAsync(id, ct);
        }
        catch (Exception e)
        {
            return NotFound(_response.FailureResponse(e.Message));
        }
        return Ok(_response.SuccessResponse(null));
    }
}
