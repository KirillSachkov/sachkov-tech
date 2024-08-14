using Microsoft.AspNetCore.Mvc;
using SachkovTech.Application.Modules.CreateModule;
using SachkovTech.Domain.Modules;

namespace SachkovTech.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ModulesController : ControllerBase
{
    public record GetFilteredModulesRequest(string name, bool Activated);

    [HttpGet]
    public ActionResult<GetAllModulesResponse> GetFilteredModules(
        [FromBody] GetFilteredModulesRequest request)
    {
        var moduleResponse = new ModuleDto(Guid.NewGuid(), "test", "test");

        var response = new GetAllModulesResponse([moduleResponse, moduleResponse, moduleResponse]);

        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetById([FromRoute] Guid id)
    {
        return Ok("module");
    }

    [HttpPut("{id:guid}")]
    public IActionResult Update([FromRoute] Guid id, [FromBody] UpdateModuleDto dto)
    {
        var request = new UpdateModuleCommand(id, dto);

        return Ok();
    }

    [HttpPut]
    public IActionResult Update()
    {
        return Ok();
    }

    [HttpDelete("{id:guid}")]
    public IActionResult DeleteById([FromRoute] Guid id)
    {
        return Ok();
    }
}