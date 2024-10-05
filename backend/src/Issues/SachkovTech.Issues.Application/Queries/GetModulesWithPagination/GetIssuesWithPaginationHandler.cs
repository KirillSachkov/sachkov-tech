using System.Data;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
using Dapper;
using Microsoft.Extensions.Logging;
using SachkovTech.Core.Abstractions;
using SachkovTech.Core.Dtos;
using SachkovTech.Core.Extensions;
using SachkovTech.Core.Models;

namespace SachkovTech.Issues.Application.Queries.GetModulesWithPagination;

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

        Expression<Func<IssueDto, object>> keySelector = query.SortBy?.ToLower() switch
        {
            "title" => (issue) => issue.Title,
            "position" => (issue) => issue.Position,
            _ => (issue) => issue.Id
        };

        issuesQuery = query.SortDirection?.ToLower() == "desc"
            ? issuesQuery.OrderByDescending(keySelector)
            : issuesQuery.OrderBy(keySelector);

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
            .ToPagedList(query.Page, query.PageSize, cancellationToken);
    }
}

public class GetIssuesWithPaginationHandlerDapper
    : IQueryHandler<PagedList<IssueDto>, GetFilteredIssuesWithPaginationQuery>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;
    private readonly ILogger<GetIssuesWithPaginationHandlerDapper> _logger;

    public GetIssuesWithPaginationHandlerDapper(ISqlConnectionFactory sqlConnectionFactory,
        ILogger<GetIssuesWithPaginationHandlerDapper> logger)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
        _logger = logger;
    }

    public async Task<PagedList<IssueDto>> Handle(
        GetFilteredIssuesWithPaginationQuery query,
        CancellationToken cancellationToken)
    {
        var connection = _sqlConnectionFactory.Create();

        var parameters = new DynamicParameters();

        var totalCount = await connection.ExecuteScalarAsync<long>("SELECT COUNT(*) FROM issues");

        var sql = new StringBuilder(
            """
              SELECT id, title, position, files FROM issues
            """);

        if (!string.IsNullOrWhiteSpace(query.Title))
        {
            sql.Append(" WHERE title = @Title");
            parameters.Add("@Title", query.Title);
        }

        sql.ApplySorting(query.SortBy, query.SortDirection);
        sql.ApplyPagination(parameters, query.Page, query.PageSize);

        var issues = await connection.QueryAsync<IssueDto, string, IssueDto>(
            sql.ToString(),
            (issue, jsonFiles) =>
            {
                var files = JsonSerializer.Deserialize<Guid[]>(jsonFiles) ?? [];

                issue.Files = files;

                return issue;
            },
            splitOn: "files",
            param: parameters);

        return new PagedList<IssueDto>
        {
            Items = issues.ToList(),
            TotalCount = totalCount,
            PageSize = query.PageSize,
            Page = query.Page,
        };
    }
}

public static class SqlExtensions
{
    public static void ApplySorting(
        this StringBuilder sqlBuilder,
        string? sortBy,
        string? sortDirection)
    {
        if (string.IsNullOrWhiteSpace(sortBy) || string.IsNullOrWhiteSpace(sortDirection)) return;

        var validSortDirections = new[] { "asc", "desc" };

        if (validSortDirections.Contains(sortDirection.ToLower()))
        {
            sqlBuilder.Append($"\norder by {sortBy} {sortDirection}");
        }
        else
        {
            throw new ArgumentException("Invalid sort parameters");
        }
    }

    public static void ApplyPagination(
        this StringBuilder sqlBuilder,
        DynamicParameters parameters,
        int page,
        int pageSize)
    {
        parameters.Add("@PageSize", pageSize, DbType.Int32);
        parameters.Add("@Offset", (page - 1) * pageSize, DbType.Int32);

        sqlBuilder.Append(" LIMIT @PageSize OFFSET @Offset");
    }
}