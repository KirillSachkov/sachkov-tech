using Microsoft.AspNetCore.Mvc;
using SachkovTech.API.Extensions;
using SachkovTech.Application.Modules.CreateModule;

namespace SachkovTech.API.Controllers;

public class ModulesController : ApplicationController
{
    [HttpPost]
    public async Task<ActionResult> Create(
        [FromServices] CreateModuleHandler handler,
        [FromBody] CreateModuleRequest request,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
}