using System.Text.Json;
using Dapper;
using SachkovTech.Application.Abstraction;
using SachkovTech.Application.Database;
using SachkovTech.Application.Dtos;
using SachkovTech.Application.Extensions;
using SachkovTech.Application.Models;
using SachkovTech.Infrastructure;

namespace SachkovTech.Application.IssueManagement.Queries.GetModulesWithPagination;

public class GetIssuesWithPaginationHandler
    : IQueryHandler<PagedList<IssueDto>, GetFilteredIssuesWithPaginationQuery>
{
    private readonly IReadDbContext _readDbContext;

    public GetIssuesWithPaginationHandler(IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public async Task<PagedList<IssueDto>> Handle(
        GetFilteredIssuesWithPaginationQuery query,
        CancellationToken cancellationToken)
    {
        var issuesQuery = _readDbContext.Issues;

        issuesQuery = issuesQuery.WhereIf(
            !string.IsNullOrWhiteSpace(query.Title),
            i => i.Title.Contains(query.Title!));

        issuesQuery = issuesQuery.WhereIf(
            query.PositionTo != null,
            i => i.Position <= query.PositionTo!.Value);

        issuesQuery = issuesQuery.WhereIf(
            query.PositionFrom != null,
            i => i.Position >= query.PositionFrom!.Value);

        return await issuesQuery
            .OrderBy(i => i.Position)
            .ToPagedList(query.Page, query.PageSize, cancellationToken);
    }
}

public class GetIssuesWithPaginationHandlerDapper
    : IQueryHandler<PagedList<IssueDto>, GetFilteredIssuesWithPaginationQuery>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public GetIssuesWithPaginationHandlerDapper(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<PagedList<IssueDto>> Handle(
        GetFilteredIssuesWithPaginationQuery query,
        CancellationToken cancellationToken)
    {
        var connection = _sqlConnectionFactory.Create();

        var parameters = new DynamicParameters();

        var totalCount = await connection.ExecuteScalarAsync<long>("SELECT COUNT(*) FROM issues");

        var sql = """
                  SELECT id, title, position, files FROM issues
                  ORDER BY position LIMIT @PageSize OFFSET @Offset
                  """;
        
        parameters.Add("@PageSize", query.PageSize);
        parameters.Add("@Offset", (query.Page - 1) * query.PageSize);

        var issues = await connection.QueryAsync<IssueDto, string, IssueDto>(
            sql,
            (issue, jsonFiles) =>
            {
                var files = JsonSerializer.Deserialize<IssueFileDto[]>(jsonFiles) ?? [];
                
                issue.Files = files;

                return issue;
            },
            splitOn: "files",
            param: parameters);

        return new PagedList<IssueDto>()
        {
            Items = issues.ToList(),
            TotalCount = totalCount,
            PageSize = query.PageSize,
            Page = query.Page,
        };
    }
}