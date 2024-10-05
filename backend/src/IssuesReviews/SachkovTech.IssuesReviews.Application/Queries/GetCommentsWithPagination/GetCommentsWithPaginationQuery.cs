using SachkovTech.Core.Abstractions;

namespace SachkovTech.IssuesReviews.Application.Queries.GetCommentsWithPagination;

public record GetCommentsWithPaginationQuery(
    string? SortDirection,
    string? SortBy,
    int Page,
    int PageSize) : IQuery
{
    private GetCommentsWithPaginationQuery(Guid issueReviewId,
        string? sortDirection,
        string? sortBy,
        int page,
        int pageSize) : this(
        sortDirection,
        sortBy,
        page,
        pageSize)
    {
        IssueReviewId = issueReviewId;
    }
    
    internal Guid IssueReviewId { get; init; }

    public GetCommentsWithPaginationQuery GetQueryWithId(Guid issueReviewId)
    {
        return new(issueReviewId, SortDirection, SortBy, Page, PageSize);
    }
}