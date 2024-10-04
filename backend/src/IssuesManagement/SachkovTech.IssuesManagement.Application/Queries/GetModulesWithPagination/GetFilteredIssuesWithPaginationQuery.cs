using SachkovTech.Core.Abstractions;

namespace SachkovTech.IssuesManagement.Application.Queries.GetModulesWithPagination;

public record GetFilteredIssuesWithPaginationQuery(
    string? Title,
    int? PositionFrom,
    int? PositionTo,
    string? SortBy,
    string? SortDirection,
    int Page,
    int PageSize) : IQuery;