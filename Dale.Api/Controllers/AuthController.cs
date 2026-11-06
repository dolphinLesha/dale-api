using Dale.Api.Contracts.Auth;
using Dale.Api.Handlers.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace Dale.Api.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController : ControllerBase
{
    private readonly IAuthHandler _authHandler;

    public AuthController(IAuthHandler authHandler)
    {
        _authHandler = authHandler;
    }

    [HttpPost("register")]
    public IActionResult Register(RegisterRequest request)
    {
        var result = _authHandler.HandleRegister(request);
        IActionResult response;

        if (result.Status == AuthStatus.Success)
        {
            response = StatusCode(
                StatusCodes.Status201Created,
                result.Tokens);
        }
        else if (result.Status == AuthStatus.EmailAlreadyUsed)
        {
            response = Conflict(new
            {
                message = "Email is already in use."
            });
        }
        else
        {
            response = BadRequest(new
            {
                message = "Invalid registration data."
            });
        }

        return response;
    }

    [HttpPost("login")]
    public IActionResult Login(LoginRequest request)
    {
        var result = _authHandler.HandleLogin(request);
        IActionResult response;

        if (result.Status == AuthStatus.Success)
        {
            response = Ok(result.Tokens);
        }
        else if (result.Status == AuthStatus.InvalidRequest)
        {
            response = BadRequest(new
            {
                message = "Email and password are required."
            });
        }
        else
        {
            response = Unauthorized(new
            {
                message = "Invalid email or password."
            });
        }

        return response;
    }

    [HttpPost("refresh")]
    public IActionResult Refresh(RefreshRequest request)
    {
        var result = _authHandler.HandleRefresh(request);
        IActionResult response;

        if (result.Status == AuthStatus.Success)
        {
            response = Ok(result.Tokens);
        }
        else
        {
            response = Unauthorized(new
            {
                message = "Invalid or expired refresh token."
            });
        }

        return response;
    }

    [Authorize]
    [HttpGet("me")]
    public IActionResult Me()
    {
        var response = new
        {
            guid = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
        };

        return Ok(response);
    }
}