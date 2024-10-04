using SachkovTech.IssuesManagement.Application.Queries.GetModulesWithPagination;

namespace SachkovTech.IssuesManagement.Presentation.Issues.Requests;

public record GetIssuesWithPaginationRequest(
    string? Title,
    int? PositionFrom,
    int? PositionTo,
    string? SortBy,
    string? SortDirection,
    int Page,
    int PageSize)
{
    public GetFilteredIssuesWithPaginationQuery ToQuery() =>
        new(Title, PositionFrom, PositionTo, SortBy, SortDirection, Page, PageSize);
}