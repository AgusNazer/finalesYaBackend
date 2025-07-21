namespace finalesYaBackend.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;

[ApiController]
[Route("api/swagger")]
public class TestSwagger : ControllerBase
{
    
    private readonly HttpClient _httpClient;

    public TestSwagger(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    [HttpGet]
    public IActionResult GetMensajeOk()
    {
        return Ok("Swagger funciona ok");
    }
// ACA IRIA LO DE LA PAI DE JSON PLACEHOLDER

// GET /api/swagger/traerPosts
    [HttpGet("traerPosts")]
    public async Task<IActionResult> GetPosts()
    {
        var response = await _httpClient.GetAsync("https://jsonplaceholder.typicode.com/posts");
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();

        // Retornar el JSON puro
        return Content(json, "application/json");
    }
}