using Microsoft.EntityFrameworkCore;
using SachkovTech.Application.Database;
using SachkovTech.Application.Dtos;
using SachkovTech.Application.Extensions;
using SachkovTech.Application.Models;

namespace SachkovTech.Application.IssueManagement.Queries.GetModulesWithPagination;

public class GetIssuesWithPaginationHandler
{
    private readonly IReadDbContext _readDbContext;

    public GetIssuesWithPaginationHandler(IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public async Task<PagedList<IssueDto>> Handle(
        GetIssuesWithPaginationQuery query,
        CancellationToken cancellationToken)
    {
        var issuesQuery = _readDbContext.Issues.AsQueryable();

        // будащая фильтрация и сортровка

        return await issuesQuery.ToPagedList(query.Page, query.PageSize, cancellationToken);
    }
}