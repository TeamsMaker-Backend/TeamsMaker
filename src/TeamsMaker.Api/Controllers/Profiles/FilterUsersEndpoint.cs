using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Contracts.QueryStringParameters;

using TeamsMaker.Api.Contracts.Responses.Profile;
using TeamsMaker.Api.Services.Profiles.Interfaces;

namespace TeamsMaker.Api.Controllers.Profiles;

[Authorize]
public class FilterUsersEndpoint(IUserService userService) : BaseApiController
{
    [Tags("profiles")]
    [Produces<List<GetUserAsRowResponse>>]
    [HttpGet("profiles")]
    public async Task<IActionResult> FilterUsers([FromQuery] UsersSearchQueryString query, CancellationToken ct)
    {
        List<GetUserAsRowResponse> users = [];

        try
        {
            users = await userService.FilterAsync(query, ct);
        }
        catch (Exception e)
        {
            return NotFound(_response.FailureResponse(e.Message));
        }

        return Ok(_response.SuccessResponse(users));
    }
}