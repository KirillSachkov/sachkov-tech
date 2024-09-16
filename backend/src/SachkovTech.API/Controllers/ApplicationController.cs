using Microsoft.AspNetCore.Mvc;
using SachkovTech.API.Response;

namespace SachkovTech.API.Controllers;

[ApiController]
[Route("[controller]")]
public abstract class ApplicationController : ControllerBase
{
    public override OkObjectResult Ok(object? value)
    {
        var envelope = Envelope.Ok(value);
        
        return base.Ok(envelope);
    }
}