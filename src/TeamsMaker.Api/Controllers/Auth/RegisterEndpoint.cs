﻿using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Contracts.Requests;
using TeamsMaker.Api.Contracts.Responses;
using TeamsMaker.Api.Services.Auth;

namespace TeamsMaker.Api.Controllers.Auth;

public class RegisterEndpoint : BaseApiController
{
    private readonly IAuthService _authService;

    public RegisterEndpoint(IAuthService authService)
    {
        _authService = authService;
    }

    [Tags("auth")]
    [Produces(typeof(TokenResponse))]
    [HttpPost("users/register")]
    public async Task<IActionResult> Register([FromBody] UserRegisterationRequest request, CancellationToken ct)
    {
        try
        {
            var token = await _authService.RegisterAsync(request, ct).ConfigureAwait(false);

            return Created("", _response.SuccessResponse(token));
        }
        catch (Exception ex)
        {
            return BadRequest(_response.FailureResponse(ex.Message));
        }
    }
}
