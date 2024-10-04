using SachkovTech.Core.Dtos;

namespace SachkovTech.Issues.Application;

public interface IReadDbContext
{
    IQueryable<ModuleDto> Modules { get; }
    IQueryable<IssueDto> Issues { get; }
}