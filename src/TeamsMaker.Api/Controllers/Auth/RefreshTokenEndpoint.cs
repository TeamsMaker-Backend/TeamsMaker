using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Contracts.Requests;
using TeamsMaker.Api.Contracts.Responses;
using TeamsMaker.Api.Services.Auth;


namespace TeamsMaker.Api.Controllers.Auth;

public class RefreshTokenEndpoint : BaseApiController
{
    private readonly IAuthService _authService;

    public RefreshTokenEndpoint(IAuthService authService)
    {
        _authService = authService;
    }

    [Tags("auth")]
    [Produces(typeof(TokenResponse))]
    [HttpPost("users/refresh_token")]
    public async Task<IActionResult> RefreshToken([FromBody] TokenRequest request, CancellationToken ct)
    {
        try
        {
            var refreshToken = await _authService.RefreshTokenAsync(request, ct).ConfigureAwait(false);

            return Ok(_response.SuccessResponse(refreshToken));
        }
        catch (Exception ex)
        {
            return BadRequest(_response.FailureResponse(ex.Message));
        }
    }
}
