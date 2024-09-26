using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SachkovTech.API.Controllers.Accounts.Requests;
using SachkovTech.API.Extensions;
using SachkovTech.Application.Authorization.Commands.Login;
using SachkovTech.Application.Authorization.Commands.Register;
using SachkovTech.Infrastructure.Authorization;

namespace SachkovTech.API.Controllers.Accounts;

public class AccountController : ApplicationController
{
    [Permission("issues.create")]
    [HttpPost("create")]
    public IActionResult CreateIssue()
    {
        return Ok();
    }

    [Permission("update.create")]
    [HttpPost("update")]
    public IActionResult UpdateIssue()
    {
        return Ok();
    }
    
    [HttpPost("registration")]
    public async Task<IActionResult> Register(
        [FromBody] RegisterUserRequest request,
        [FromServices] RegisterUserHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request.ToCommand(), cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();
    
        return Ok();
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginUserRequest request,
        [FromServices] LoginHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request.ToCommand(), cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();
    
        return Ok(result.Value);
    }
}