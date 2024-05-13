using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Contracts.Requests.Post;
using TeamsMaker.Api.Services.Posts.Interfaces;

namespace TeamsMaker.Api.Controllers.Posts;

public class UpdatePostEndpoint(IPostService postService) : BaseApiController
{
    [Tags("posts")]
    [HttpPatch("posts/{id}")]
    public async Task<IActionResult> UpdatePost(Guid id, UpdatePostRequest request, CancellationToken ct)
    {
        try
        {
            await postService.UpdateAsync(id, request, ct);
        }
        catch (Exception e)
        {
            return NotFound(_response.FailureResponse(e.Message));
        }

        return Ok(_response.SuccessResponse(null));
    }
}
