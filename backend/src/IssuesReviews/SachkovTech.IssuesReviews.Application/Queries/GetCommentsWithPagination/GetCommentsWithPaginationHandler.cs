using System.Linq.Expressions;
using SachkovTech.Core.Abstractions;
using SachkovTech.Core.Dtos;
using SachkovTech.Core.Extensions;
using SachkovTech.Core.Models;

namespace SachkovTech.IssuesReviews.Application.Queries.GetCommentsWithPagination;

public class GetCommentsWithPaginationHandler: IQueryHandler<PagedList<CommentDto>, GetCommentsWithPaginationQuery>
{
    private readonly IReadDbContext _readDbContext;

    public GetCommentsWithPaginationHandler(IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public async Task<PagedList<CommentDto>> Handle(
        GetCommentsWithPaginationQuery query,
        CancellationToken cancellationToken)
    {
        var commentsQuery = _readDbContext.Comments
            .Where(c => c.IssueReviewId == query.IssueReviewId);
        
        Expression<Func<CommentDto, object>> keySelector = query.SortBy?.ToLower() switch
        {
            "createdat" => (commentDto) => commentDto.CreatedAt,
            "message" => (commentDto) => commentDto.Message,
            _ => (commentDto) => commentDto.UserId
        };
        
        commentsQuery = query.SortDirection?.ToLower() == "desc"
            ? commentsQuery.OrderByDescending(keySelector)
            : commentsQuery.OrderBy(keySelector);
        
        return await commentsQuery
            .ToPagedList(query.Page, query.PageSize, cancellationToken);
    }
}