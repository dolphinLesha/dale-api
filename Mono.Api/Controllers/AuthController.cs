using Microsoft.AspNetCore.Mvc;
using Mono.Api.Contracts.Auth;
using Mono.Api.Handlers.Auth;

namespace Mono.Api.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController : ControllerBase
{
    private readonly IAuthHandler _registerHandler;

    public AuthController(IAuthHandler registerHandler)
    {
        _registerHandler = registerHandler;
    }

    [HttpPost("register")]
    public IActionResult Register(RegisterRequest request)
    {
        var result = _registerHandler.HandleRegister(request);
        IActionResult response;

        if (result.IsEmailAlreadyUsed)
        {
            response = Conflict(new
            {
                message = "Email is already in use."
            });
        }
        else
        {
            response = Created($"/api/users/{result.Response.Guid}", result.Response);
        }

        return response;
    }
}