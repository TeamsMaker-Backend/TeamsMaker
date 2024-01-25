using Core.Generics;
using Microsoft.AspNetCore.Mvc;
namespace TeamsMaker.Api.Controllers.Auth;


public class LoginEndpoint : BaseApiController
{
    private readonly IAuthService _authService;

    public LoginEndpoint(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("user/login")]
    public async Task<IActionResult> Login(UserLoginRequest request, CancellationToken ct)
    {
        await _authService.LoginAsync(request, ct);
        throw new NotImplementedException();
    }
}
