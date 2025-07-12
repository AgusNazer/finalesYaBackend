namespace finalesYaBackend.Controllers;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/swagger")]
public class TestSwagger : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("Swagger funciona ok");
    }
}