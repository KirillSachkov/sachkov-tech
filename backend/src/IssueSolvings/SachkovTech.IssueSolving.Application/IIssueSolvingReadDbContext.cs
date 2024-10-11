using SachkovTech.Core.Dtos;

namespace SachkovTech.IssueSolving.Application;

public interface IIssueSolvingReadDbContext
{
    IQueryable<UserIssueDto> UserIssues { get; }
}