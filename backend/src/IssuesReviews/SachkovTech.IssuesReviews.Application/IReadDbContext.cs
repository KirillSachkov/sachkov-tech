using SachkovTech.Core.Dtos;

namespace SachkovTech.IssuesReviews.Application;

public interface IReadDbContext
{
    IQueryable<IssueReviewDto> IssueReviewDtos { get; }
    IQueryable<CommentDto> Comments { get; }
}

public class ReadDbContext : IReadDbContext
{
    public IQueryable<CommentDto> Comments { get; }
}