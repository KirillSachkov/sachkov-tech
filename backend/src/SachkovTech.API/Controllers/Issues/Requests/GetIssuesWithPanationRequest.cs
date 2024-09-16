using SachkovTech.Application.IssueManagement.Queries.GetModulesWithPagination;

namespace SachkovTech.API.Controllers.Issues.Requests;

public record GetIssuesWithPaginationRequest(int Page, int PageSize)
{
    public GetIssuesWithPaginationQuery ToQuery() =>
        new(Page, PageSize);
}