using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Services.Auth;
namespace TeamsMaker.Api.Controllers.Auth;


public class LoginEndpoint : BaseApiController
{
    private readonly IAuthService _authService;

    public LoginEndpoint(IAuthService authService)
    {
        _authService = authService;
    }

    [Tags("Auth")]
    [HttpPost("users/login")]
    public async Task<IActionResult> Login(UserLoginRequest request, CancellationToken ct)
    {
        try
        {
            var token = await _authService.LoginAsync(request, ct).ConfigureAwait(false);

            return Ok(_response.SuccessResponse(token));
        }
        catch (Exception ex)
        {
            return BadRequest(_response.FailureResponse(ex.Message));
        }
    }
}
