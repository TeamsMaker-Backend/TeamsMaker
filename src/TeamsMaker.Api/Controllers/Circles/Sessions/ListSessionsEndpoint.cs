﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Contracts.QueryStringParameters;
using TeamsMaker.Api.Services.Circles.Interfaces;

namespace TeamsMaker.Api.Controllers.Circles.Sessions;

[Authorize]
public class ListSessionsEndpoint(ISessionService sessionService) : BaseApiController
{
    [Tags("circles/sessions")]
    [HttpGet("circles/{id}/sessions/{status}")]
    public async Task<IActionResult> Sessions(Guid id, SessionStatus status, [FromQuery] SessionsQueryString queryString, CancellationToken ct)
    {
        try
        {
            var sessions = await sessionService.ListAsync(id, status, queryString, ct);
            return sessions is not null ? Ok(_response.SuccessResponse(sessions)) : NotFound();
        }
        catch (ArgumentException e)
        {
            return NotFound(_response.FailureResponse(e.Message));
        }
    }
}