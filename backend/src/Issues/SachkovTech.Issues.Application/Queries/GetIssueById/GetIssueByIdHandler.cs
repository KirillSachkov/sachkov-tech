using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using SachkovTech.Core.Abstractions;
using SachkovTech.Core.Dtos;
using SachkovTech.SharedKernel;

namespace SachkovTech.Issues.Application.Queries.GetIssueById;

public class GetIssueByIdHandler : IQueryHandlerWithResult<IssueDto, GetIssueByIdQuery>
{
    private readonly IReadDbContext _readDbContext;

    public GetIssueByIdHandler(IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public async Task<Result<IssueDto, ErrorList>> Handle(
        GetIssueByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        var issueDto = await _readDbContext.Issues
            .FirstOrDefaultAsync(i => i.Id == query.IssueId, cancellationToken);

        if (issueDto is null)
            return Errors.General.NotFound(query.IssueId).ToErrorList();

        return issueDto;
    }
}