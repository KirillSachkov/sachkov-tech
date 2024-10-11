using SachkovTech.Core.Dtos;

namespace SachkovTech.IssuesReviews.Application;

public interface IReadDbContext
{
    IQueryable<IssueReviewDto> IssueReviewDtos { get; }
    IQueryable<CommentDto> Comments { get; }
}
