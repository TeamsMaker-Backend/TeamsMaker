using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Contracts.Responses.Profile;
using TeamsMaker.Api.Services.Profiles.Interfaces;
using TeamsMaker.Core.Enums;

namespace TeamsMaker.Api.Controllers.Profiles;

[Authorize]
public class GetOtherProfileEndpoint(IServiceProvider serviceProvider) : BaseApiController
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    [Tags("profiles")]
    [HttpGet("profiles/{userEnum}/{id}")]
    public async Task<IActionResult> Profile(UserEnum userEnum, string id, CancellationToken ct)
    {
        var profileService = _serviceProvider.GetRequiredKeyedService<IProfileService>(userEnum);
        GetOtherProfileResponse response;

        try
        {
            response = await profileService.GetOtherProfileAsync(id, ct);
        }
        catch (ArgumentException e)
        {
            return NotFound(_response.FailureResponse(e.Message));
        }

        return Ok(_response.SuccessResponse(response));
    }
}
