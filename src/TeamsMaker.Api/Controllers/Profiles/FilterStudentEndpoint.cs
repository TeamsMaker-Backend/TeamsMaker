using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Contracts.Responses.Profile;
using TeamsMaker.Api.Services.Profiles.Interfaces;
using TeamsMaker.Core.Enums;

namespace TeamsMaker.Api.Controllers.Profiles;

[Authorize]
public class FilterStudentEndpoint(IServiceProvider serviceProvider) : BaseApiController
{
    [Tags("profiles")]
    [HttpGet("profiles/students")]
    public async Task<IActionResult> FilterStudent([FromQuery] string query, CancellationToken ct)
    {
        var profileService = serviceProvider.GetRequiredKeyedService<IProfileService>(UserEnum.Student);
        List<GetStudentAsRowResponse> response = [];

        try
        {
            response = await profileService.FilterAsync(query, ct);
        }
        catch (ArgumentException e)
        {
            return NotFound(_response.FailureResponse(e.Message));
        }

        return Ok(_response.SuccessResponse(null));
    }
}