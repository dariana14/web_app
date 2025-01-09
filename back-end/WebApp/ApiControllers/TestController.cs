using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.ApiControllers;

[ApiController]
[Route("/api/[controller]")]
[Authorize(AuthenticationSchemes = "Identity.Application,Bearer")]

public class TestController: ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<string>>> Get()
    {
        return Ok(new List<string>(){"aa", "bb", "cc"});
    }
}