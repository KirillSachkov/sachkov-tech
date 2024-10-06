using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using SachkovTech.Core.Abstractions;
using SachkovTech.Core.Dtos;
using SachkovTech.SharedKernel;

namespace SachkovTech.Issues.Application.Queries.GetIssueByPosition;

public class GetIssueByPositionHandler : IQueryHandlerWithResult<IssueDto, GetIssueByPositionQuery>
{
    private readonly IReadDbContext _readDbContext;

    public GetIssueByPositionHandler(IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public async Task<Result<IssueDto, ErrorList>> Handle(
        GetIssueByPositionQuery query,
        CancellationToken cancellationToken = default)
    {
        var issueDto = await _readDbContext.Issues
            .FirstOrDefaultAsync(i => i.Position == query.Position, cancellationToken);

        if (issueDto is null)
            return Errors.General.NotFound().ToErrorList();

        return issueDto;
    }
}