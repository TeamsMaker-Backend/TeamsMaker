﻿using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Services.Files.Interfaces;

namespace TeamsMaker.Api.Controllers.Files;

//TODO: to be authorized
public class GetFileEndpoint(IServiceProvider serviceProvider) : BaseApiController
{
    [Tags("files")]
    [Produces<FileContentResult>]
    [HttpGet("files/{baseType}/{id}/{fileType}")]
    [ResponseCache(Duration = 60)]
    public async Task<IActionResult> File(string baseType, string id, string fileType, CancellationToken ct)
    {
        var fileService = serviceProvider.GetRequiredKeyedService<IFileService>(baseType);

        try
        {
            var result = await fileService.GetFileContentAsync(id, fileType, ct);
            return result is not null ? result : NotFound();
        }
        catch (Exception e)
        {
            return NotFound(_response.FailureResponse(e.Message));
        }
    }
}
