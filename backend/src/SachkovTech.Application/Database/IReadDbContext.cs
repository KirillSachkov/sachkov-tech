using SachkovTech.Application.Dtos;

namespace SachkovTech.Application.Database;

public interface IReadDbContext
{
    IQueryable<ModuleDto> Modules { get; }
    IQueryable<IssueDto> Issues { get; }
}