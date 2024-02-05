﻿using Core.Generics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeamsMaker.Api.Contracts.Requests;
using TeamsMaker.Api.Services.Auth;


namespace TeamsMaker.Api.Controllers.Auth;

[Authorize]
public class RefreshTokenEndpoint : BaseApiController
{
    private readonly IAuthService _authService;

    public RefreshTokenEndpoint(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("users/refresh_token")]
    public async Task<IActionResult> RefreshToken(TokenRequest request, CancellationToken ct)
    {
        var refreshToken = await _authService.RefreshTokenAsync(request, ct).ConfigureAwait(false);

        return Ok(_response.SuccessResponse(refreshToken));
    }
}
