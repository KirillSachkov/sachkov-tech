using SachkovTech.Core.Dtos;

namespace SachkovTech.IssuesReviews.Application;

public interface IReadDbContext
{
    IQueryable<CommentDto> Comments { get; }
}