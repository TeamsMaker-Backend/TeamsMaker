using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Contracts.Requests.Post;
using TeamsMaker.Api.Contracts.Responses;
using TeamsMaker.Api.Services.Posts.Interfaces;

namespace TeamsMaker.Api.Controllers.Posts
{
    [Authorize]
    public class AddPostEndpoint(IPostService postService) :BaseApiController
    {
        [Tags("post")]
        [Produces<IdResponse<Guid>>]
        [HttpPost("post")]
        public async Task<IActionResult> AddPost(AddPostRequest request, CancellationToken ct)
        {
            Guid postId;
            try
            {
                postId = await postService.AddAsync(request, ct);
            }
            catch (Exception e)
            {
                return NotFound(_response.FailureResponse(e.Message));
            }
            return Created("", _response.SuccessResponse(new IdResponse<Guid>(postId)));
        }
    }
}
