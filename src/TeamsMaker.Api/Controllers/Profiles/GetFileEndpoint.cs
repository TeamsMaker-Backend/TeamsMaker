using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Services.Profiles.Interfaces;
using TeamsMaker.Core.Enums;

namespace TeamsMaker.Api.Controllers.Profiles;

public class GetFileEndpoint(IServiceProvider serviceProvider) : BaseApiController
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private IProfileService? _profileService;

    [HttpGet("profiles/{id}/{userEnum}/files/{fileType}")]
    public async Task<IActionResult> File(Guid id, UserEnum userEnum, string fileType, CancellationToken ct)
    {
        _profileService = _serviceProvider.GetRequiredKeyedService<IProfileService>(userEnum);
        FileContentResult? result;

        try
        {
            result = await _profileService.GetFileContentAsync(id, fileType, ct);
        }
        catch (ArgumentException)
        {
            return NotFound(_response.FailureResponse("Invalid Data"));
        }

        return Ok(_response.SuccessResponse(result));
    }
}
