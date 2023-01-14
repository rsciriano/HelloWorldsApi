using Microsoft.AspNetCore.Mvc;

namespace Net7ClassicApi.Controllers;

[ApiController]
[Route("api/hello")]
public class HelloController : ControllerBase
{
    [HttpGet]
    public string Get(string name)
    {
        return $"Hello {name ?? "world"}!";
    }
}

