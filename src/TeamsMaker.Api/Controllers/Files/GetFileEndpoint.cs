﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Services.Files.Interfaces;

namespace TeamsMaker.Api.Controllers.Files;

[Authorize]
public class GetFileEndpoint(IServiceProvider serviceProvider) : BaseApiController
{
    [Tags("files")]
    [HttpGet("files/{baseType}/{id}/{fileType}")]
    public async Task<IActionResult> File(string baseType, string id, string fileType, CancellationToken ct)
    {
        var fileService = serviceProvider.GetRequiredKeyedService<IFileService>(baseType);
        FileContentResult? result;

        try
        {
            result = await fileService.GetFileContentAsync(id, fileType, ct);
        }
        catch (ArgumentException e)
        {
            return NotFound(_response.FailureResponse(e.Message));
        }

        return Ok(_response.SuccessResponse(result));
    }
}
