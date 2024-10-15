using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SachkovTech.Files.Contracts.Converters;
using SachkovTech.Framework;
using SachkovTech.Issues.Application.Commands.AddIssue;
using SachkovTech.Issues.Application.Commands.Create;
using SachkovTech.Issues.Application.Commands.Delete;
using SachkovTech.Issues.Application.Commands.DeleteIssue;
using SachkovTech.Issues.Application.Commands.DeleteIssue.SoftDeleteIssue;
using SachkovTech.Issues.Application.Commands.ForceDeleteIssue;
using SachkovTech.Issues.Application.Commands.RestoreIssue;
using SachkovTech.Issues.Application.Commands.UpdateIssueMainInfo;
using SachkovTech.Issues.Application.Commands.UpdateIssuePosition;
using SachkovTech.Issues.Application.Commands.UpdateMainInfo;
using SachkovTech.Issues.Application.Commands.UploadFilesToIssue;
using SachkovTech.Issues.Presentation.Modules.Requests;
using SachkovTech.Issues.Presentation.Modules.Responses;

namespace SachkovTech.Issues.Presentation.Modules;

public class ModulesController : ApplicationController
{
    [HttpPost]
    public async Task<ActionResult> Create(
        [FromServices] CreateModuleHandler handler,
        [FromBody] CreateModuleRequest request,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request.ToCommand(), cancellationToken);

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

    [HttpPost("{moduleId:guid}/issue/{issueId:guid}/files")]
    public async Task<ActionResult> UploadFilesToIssue(
        [FromRoute] Guid moduleId,
        [FromRoute] Guid issueId,
        [FromForm] IFormFileCollection files,
        [FromServices] UploadFilesToIssueHandler handler,
        [FromServices] IFormFileConverter fileConverter,
        CancellationToken cancellationToken)
    {
        var fileDtos = fileConverter.ToUploadFileDtos(files);

        var command = new UploadFilesToIssueCommand(moduleId, issueId, fileDtos);

        var result = await handler.Handle(command, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        var response = UploadFilesToIssueResponse
            .MapFromUploadFilesResult(result.Value);

        return Ok(response);
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
    
    [HttpPut("{id:guid}/issue/{issueId:guid}/newPosition/{newPosition:int}")]
    public async Task<ActionResult> UpdateIssuePosition(
        [FromRoute] Guid id,
        [FromRoute] Guid issueId,
        [FromRoute] int newPosition,
        [FromServices] UpdateIssuePositionHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new UpdateIssuePositionCommand(id, issueId, newPosition);
        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPut("{id:guid}/issue/{issueId:guid}/main-info")]
    public async Task<ActionResult> UpdateIssueMainInfo(
        [FromRoute] Guid id,
        [FromRoute] Guid issueId,
        [FromBody] UpdateIssueMainInfoRequest request,
        [FromServices] UpdateIssueMainInfoHandler handler,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand(id, issueId);
        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPut("{moduleId:guid}/issue/{issueId:guid}/restore")]
    public async Task<ActionResult> RestoreIssue(
        [FromRoute] Guid moduleId,
        [FromRoute] Guid issueId,
        [FromServices] RestoreIssueHandler handler,
        CancellationToken cancellationToken = default)
    {
        var command = new RestoreIssueCommand(moduleId, issueId);

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

    [HttpDelete("{id:guid}/issue/{issueId:guid}/soft")]
    public async Task<ActionResult> SoftDeleteIssue(
        [FromRoute] Guid id,
        [FromRoute] Guid issueId,
        [FromServices] SoftDeleteIssueHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new DeleteIssueCommand(id, issueId);
        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpDelete("{id:guid}/issue/{issueId:guid}/force")]
    public async Task<ActionResult> ForceDeleteIssue(
        [FromRoute] Guid id,
        [FromRoute] Guid issueId,
        [FromServices] ForceDeleteIssueHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new DeleteIssueCommand(id, issueId);
        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
}