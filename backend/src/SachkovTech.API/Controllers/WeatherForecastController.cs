using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using SachkovTech.Domain.Module;

namespace SachkovTech.API.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    [HttpGet]
    public IActionResult Get(string title, string description)
    {
        var moduleResult = Module.Create(title, description);

        if (moduleResult.IsFailure)
            return BadRequest(moduleResult.Error);

        var result = Save(moduleResult.Value);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok();
    }

    public Result Save(Module module)
    {
        if (true)
        {
            return Result.Success();
        }

        return Result.Failure("fdjslkfsj");
    }
}