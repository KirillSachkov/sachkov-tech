using SachkovTech.Core.Dtos;

namespace SachkovTech.IssuesManagement.Application;

public interface IReadDbContext
{
    IQueryable<ModuleDto> Modules { get; }
    IQueryable<IssueDto> Issues { get; }
}