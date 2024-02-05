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

    [HttpPost("users/verify")] //TODO: as a staff or student
    public async Task<IActionResult> Verify([FromQuery] int userType, [FromBody] UserVerificationRequset request, CancellationToken ct)
    {
        var result = await _authService.VerifyUserAsync(request, ct).ConfigureAwait(false);
        
        throw new NotImplementedException();
    }
}
