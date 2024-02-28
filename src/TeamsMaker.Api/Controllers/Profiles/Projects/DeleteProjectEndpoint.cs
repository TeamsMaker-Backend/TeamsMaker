﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Services.Profiles.Interfaces;

namespace TeamsMaker.Api.Controllers.Profiles.Projects;

[Authorize]
public class DeleteProjectEndpoint(IProjectService projectService) : BaseApiController
{
    private readonly IProjectService _projectService = projectService;

    [Tags("profiles/Projects")]
    [HttpDelete("profiles/projects/{id}")]
    public async Task<IActionResult> Project(int id, CancellationToken ct)
    {
        try
        {
            await _projectService.DeleteProjectAsync(id, ct);
        }
        catch (ArgumentException)
        {
            return NotFound(_response.FailureResponse("Invalid Data"));
        }
        return Ok();
    }
}