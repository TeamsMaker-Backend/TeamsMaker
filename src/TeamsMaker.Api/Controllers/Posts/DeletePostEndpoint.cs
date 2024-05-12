using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Services.Posts.Interfaces;
using TeamsMaker.Api.Services.Reacts;

namespace TeamsMaker.Api.Controllers.Posts
{
    public class DeletePostEndpoint(IPostService postService) : BaseApiController
    {
        [Tags("post")]
        [HttpDelete("post")]
        public async Task<IActionResult> DeletePost([FromQuery] Guid id, CancellationToken ct)
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
}
