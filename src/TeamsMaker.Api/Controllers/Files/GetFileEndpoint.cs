using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Services.Files.Interfaces;

namespace TeamsMaker.Api.Controllers.Files;

[Authorize]
public class GetFileEndpoint(IServiceProvider serviceProvider) : BaseApiController
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    [Tags("files")]
    [HttpGet("files/{baseType}/{id}/{fileType}")]
    public async Task<IActionResult> File(string baseType, Guid id, string fileType, CancellationToken ct)
    {
        var fileService = _serviceProvider.GetRequiredKeyedService<IFileService>(baseType);
        FileContentResult? result;

        try
        {
            result = await fileService.GetFileContentAsync(id, fileType, ct);
        }
        catch (ArgumentException)
        {
            return NotFound(_response.FailureResponse("Invalid Data"));
        }

        return Ok(_response.SuccessResponse(result));
    }
}
