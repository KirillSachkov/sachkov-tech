using Microsoft.EntityFrameworkCore;
using SachkovTech.Application.Dtos;

namespace SachkovTech.Application.Database;

public interface IReadDbContext
{
    DbSet<ModuleDto> Modules { get; }
    DbSet<IssueDto> Issues { get; }
}