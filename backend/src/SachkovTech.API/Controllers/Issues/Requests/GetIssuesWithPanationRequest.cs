using SachkovTech.Application.IssueManagement.Queries.GetModulesWithPagination;

namespace SachkovTech.API.Controllers.Issues.Requests;

public record GetIssuesWithPaginationRequest(
    string? Title, int? PositionFrom, int? PositionTo, int Page, int PageSize)
{
    public GetFilteredIssuesWithPaginationQuery ToQuery() =>
        new(Title, PositionFrom, PositionTo, Page, PageSize);
}