using Microsoft.AspNetCore.Mvc;
using SachkovTech.Domain.Modules;

namespace SachkovTech.API.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    [HttpGet]
    public IActionResult Get(string title, string description)
    {
        var moduleResult = Module.Create(ModuleId.NewModuleId(), title, description);

        if (moduleResult.IsFailure)
            return BadRequest(moduleResult.Error);

        return Ok();
    }
}