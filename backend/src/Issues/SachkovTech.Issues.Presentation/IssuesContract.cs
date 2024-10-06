using CSharpFunctionalExtensions;
using SachkovTech.Core.Dtos;
using SachkovTech.Issues.Application.Queries.GetIssueById;
using SachkovTech.Issues.Application.Queries.GetIssueByPosition;
using SachkovTech.Issues.Contracts;
using SachkovTech.SharedKernel;

namespace SachkovTech.Issues.Presentation;

public class IssuesContract : IIssuesContract
{
    private readonly GetIssueByIdHandler _getIssueByIdHandler;
    private readonly GetIssueByPositionHandler _getIssueByPositionHandler;
    
    public IssuesContract(
        GetIssueByIdHandler getIssueByIdHandler,
        GetIssueByPositionHandler getIssueByPositionHandler)
    {
        _getIssueByIdHandler = getIssueByIdHandler;
        _getIssueByPositionHandler = getIssueByPositionHandler;
    }
    
    public async Task<Result<IssueDto, ErrorList>> GetIssueById(
        Guid issueId, 
        CancellationToken cancellationToken = default)
    {
        var query = new GetIssueByIdQuery(issueId);
        
        return await _getIssueByIdHandler.Handle(query, cancellationToken);
    }

    public async Task<Result<IssueDto, ErrorList>> GetIssueByPosition(
        int position, 
        CancellationToken cancellationToken = default)
    {
        var query = new GetIssueByPositionQuery(position);
        
        return await _getIssueByPositionHandler.Handle(query, cancellationToken);
    }
}