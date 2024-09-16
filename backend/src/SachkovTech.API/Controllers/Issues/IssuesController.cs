using Microsoft.AspNetCore.Mvc;
using SachkovTech.API.Controllers.Issues.Requests;
using SachkovTech.Application.IssueManagement.Queries.GetModulesWithPagination;

namespace SachkovTech.API.Controllers.Issues;

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
}