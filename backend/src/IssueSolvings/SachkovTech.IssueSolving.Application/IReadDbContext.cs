using SachkovTech.Core.Dtos;

namespace SachkovTech.IssueSolving.Application;

public interface IReadDbContext
{
    IQueryable<UserIssueDto> UserIssues { get; }
}