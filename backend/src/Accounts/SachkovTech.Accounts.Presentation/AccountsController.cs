using Microsoft.AspNetCore.Mvc;
using SachkovTech.Accounts.Application.Commands.Login;
using SachkovTech.Accounts.Contracts.Requests;
using SachkovTech.Emails;
using SachkovTech.Framework;
using SachkovTech.Framework.Authorization;

namespace SachkovTech.Accounts.Presentation;

public class AccountsController : ApplicationController
{
    [Permission(Permissions.Issues.CreateIssue)]
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

    // [HttpPost("registration")]
    // public async Task<IActionResult> Register(
    //     [FromBody] RegisterUserRequest request,
    //     [FromServices] RegisterUserHandler handler,
    //     CancellationToken cancellationToken)
    // {
    //     var result = await handler.Handle(
    //         new RegisterUserCommand(request.Email, request.UserName, request.Password),
    //         cancellationToken);
    //
    //     if (result.IsFailure)
    //         return result.Error.ToResponse();
    //
    //     return Ok();
    // }

    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginUserRequest request,
        [FromServices] LoginHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(
            new LoginCommand(request.Email, request.Password),
            cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    } 
    
    [HttpPost("email-test")]
    public async Task<IActionResult> Email(CancellationToken cancellationToken)
    {
        var emailService = new EmailSender();

        await emailService.SendEmailAsync(new EmailData("kirillvirtul@yandex.ru", "подтверджение почты", "сообщение"));

        return Ok();
    }
}