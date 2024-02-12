using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Contracts.Requests;
using TeamsMaker.Api.Services.Auth;

namespace TeamsMaker.Api.Controllers.Auth;

public class VerifyUserEndpoint : BaseApiController
{
    private readonly IAuthService _authService;

    public VerifyUserEndpoint(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("users/verify")]
    public async Task<IActionResult> Verify([FromBody] UserVerificationRequset request, CancellationToken ct)
    {
        var isVerified = await _authService.VerifyUserAsync(request, ct).ConfigureAwait(false);

        return Ok(_response.SuccessResponse(isVerified));
    }
}
