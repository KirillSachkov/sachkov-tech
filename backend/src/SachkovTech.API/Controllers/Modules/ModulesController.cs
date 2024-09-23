using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SachkovTech.API.Controllers.Modules.Requests;
using SachkovTech.API.Extensions;
using SachkovTech.API.Processors;
using SachkovTech.Application.IssueManagement.Commands.AddIssue;
using SachkovTech.Application.IssueManagement.Commands.Create;
using SachkovTech.Application.IssueManagement.Commands.Delete;
using SachkovTech.Application.IssueManagement.Commands.UpdateMainInfo;
using SachkovTech.Application.IssueManagement.Commands.UploadFilesToIssue;

namespace SachkovTech.API.Controllers.Modules;

public class ModulesController : ApplicationController
{
    [HttpPost("jwt")]
    public ActionResult Login(CancellationToken cancellationToken)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, "userId"),
            new Claim("Kirill", "Sachkov"),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("fdasfasdfsfadsfasddfsafasdffdasfdsfasfasdfsd"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "test",
            audience: "test",
            claims: claims,
            signingCredentials: creds
        );

        var user = HttpContext.User;

        var stringToken = new JwtSecurityTokenHandler().WriteToken(token);

        return Ok(stringToken);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult> Create(
        [FromServices] CreateModuleHandler handler,
        [FromBody] CreateModuleRequest request,
        CancellationToken cancellationToken)
    {
        var user = HttpContext.User;
        
        var result = await handler.Handle(request.ToCommand(), cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPut("{id:guid}/main-info")]
    public async Task<ActionResult> UpdateMainInfo(
        [FromRoute] Guid id,
        [FromBody] UpdateMainInfoRequest request,
        [FromServices] UpdateMainInfoHandler handler,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand(id);
        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete(
        [FromRoute] Guid id,
        [FromServices] DeleteModuleHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new DeleteModuleCommand(id);
        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPost("{id:guid}/issue")]
    public async Task<ActionResult> AddIssue(
        [FromRoute] Guid id,
        [FromBody] AddIssueRequest request,
        [FromServices] AddIssueHandler handler,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand(id);

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPost("{id:guid}/issue/{issueId:guid}/files")]
    public async Task<ActionResult> UploadFilesToIssue(
        [FromRoute] Guid id,
        [FromRoute] Guid issueId,
        [FromForm] IFormFileCollection files,
        [FromServices] UploadFilesToIssueHandler handler,
        CancellationToken cancellationToken)
    {
        await using var fileProcessor = new FormFileProcessor();
        var fileDtos = fileProcessor.Process(files);

        var command = new UploadFilesToIssueCommand(id, issueId, fileDtos);

        var result = await handler.Handle(command, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
}