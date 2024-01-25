using Core.Generics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace TeamsMaker.Api;

[ApiController]
[Route("api/")]
public abstract class BaseApiController : ControllerBase
{
    protected Response<object> _response { get; } = new();
}
