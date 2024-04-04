using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Contracts.Requests.File;
using TeamsMaker.Api.Services.Files.Interfaces;

namespace TeamsMaker.Api.Controllers.Files;

[Authorize]
public class UpdateFileEndpoint(IServiceProvider serviceProvider) : BaseApiController
{
    [Tags("files")]
    [HttpPut("files/{baseType}/{id}/{fileType}")]
    public async Task<IActionResult> File(string baseType, string id, string fileType, [FromForm] UpdateFileRequest request, CancellationToken ct)
    {
        var fileService = serviceProvider.GetRequiredKeyedService<IFileService>(baseType);

        try
        {
            await fileService.UpdateFileAsync(id, fileType, request, ct);
        }
        catch (ArgumentException e)
        {
            return NotFound(_response.FailureResponse(e.Message));
        }

        return Ok(_response.SuccessResponse(null));
    }
}
