using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Services.Profiles.Interfaces;
using TeamsMaker.Core.Enums;

namespace TeamsMaker.Api.Controllers.Profiles;

public class GetFileEndpoint(IServiceProvider serviceProvider) : BaseApiController
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    [Tags("profiles")]
    [HttpGet("profiles/{userEnum}/{id}/files/{fileType}")]
    public async Task<IActionResult> File(UserEnum userEnum, Guid id, string fileType, CancellationToken ct)
    {
        var profileService = _serviceProvider.GetRequiredKeyedService<IProfileService>(userEnum);
        FileContentResult? result;

        try
        {
            result = await profileService.GetFileContentAsync(id, fileType, ct);
        }
        catch (ArgumentException)
        {
            return NotFound(_response.FailureResponse("Invalid Data"));
        }

        return Ok(_response.SuccessResponse(result));
    }
}
