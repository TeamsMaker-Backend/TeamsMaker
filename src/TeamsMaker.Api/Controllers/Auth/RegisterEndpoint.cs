
namespace TeamsMaker.Api.Controllers.Auth;

public class RegisterEndpoint : BaseApiController
{
    private readonly UserManager<User> _userManager;

    public RegisterEndpoint(UserManager<User> userManager)
    {
        _userManager = userManager;
    }


}
