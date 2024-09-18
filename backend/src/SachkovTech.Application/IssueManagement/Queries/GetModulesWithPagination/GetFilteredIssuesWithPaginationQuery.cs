using SachkovTech.Application.Abstraction;

namespace SachkovTech.Application.IssueManagement.Queries.GetModulesWithPagination;

public record GetFilteredIssuesWithPaginationQuery(
    string? Title,
    int? PositionFrom,
    int? PositionTo,
    string? SortBy,
    string? SortDirection,
    int Page,
    int PageSize) : IQuery;