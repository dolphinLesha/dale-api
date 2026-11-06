using Microsoft.AspNetCore.Mvc;

namespace Dale.Api.Controllers;

[ApiController]
[Route("api/health")]
public sealed class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        var response = new
        {
            status = "ok",
            service = "dale-api"
        };

        return Ok(response);
    }
}