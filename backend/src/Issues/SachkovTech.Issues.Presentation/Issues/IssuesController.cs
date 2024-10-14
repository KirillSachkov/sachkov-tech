using Microsoft.AspNetCore.Mvc;
using SachkovTech.Framework;
using SachkovTech.Issues.Application.Queries.GetIssueById;
using SachkovTech.Issues.Application.Queries.GetModulesWithPagination;
using SachkovTech.Issues.Presentation.Issues.Requests;

namespace SachkovTech.Issues.Presentation.Issues;

public class IssuesController : ApplicationController
{
    [HttpGet("dapper")]
    public async Task<ActionResult> GetDapper(
        [FromQuery] GetIssuesWithPaginationRequest request,
        [FromServices] GetIssuesWithPaginationHandlerDapper handler,
        CancellationToken cancellationToken)
    {
        var query = request.ToQuery();
        
        var response = await handler.Handle(query, cancellationToken);
        
        return Ok(response);
    }
    
    [HttpGet]
    public async Task<ActionResult> Get(
        [FromQuery] GetIssuesWithPaginationRequest request,
        [FromServices] GetIssuesWithPaginationHandler handler,
        CancellationToken cancellationToken)
    {
        var query = request.ToQuery();
        
        var response = await handler.Handle(query, cancellationToken);
        
        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult> GetById(
        [FromRoute] Guid id,
        [FromServices] GetIssueByIdHandler handler,
        CancellationToken cancellationToken)
    {
        var query = new GetIssueByIdQuery(id);

        var response = await handler.Handle(query, cancellationToken);

        if (response.IsFailure)
            return response.Error.ToResponse();

        return Ok(response.Value);
    }
}