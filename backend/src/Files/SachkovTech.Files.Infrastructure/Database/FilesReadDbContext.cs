using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SachkovTech.Core.Dtos;
using SachkovTech.Files.Application.Interfaces;

namespace SachkovTech.Files.Infrastructure.Database
{
    internal class FilesReadDbContext : DbContext, IFilesReadDbContext
    {
        private readonly IConfiguration _configuration;
        private readonly ILoggerFactory _loggeFactory;


        public FilesReadDbContext(IConfiguration configuration, ILoggerFactory loggeFactory)
        {
            _configuration = configuration;
            _loggeFactory = loggeFactory;
        }

        public IQueryable<FileDto> Files => Set<FileDto>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {


            optionsBuilder.UseNpgsql(_configuration.GetConnectionString("Database"));
            optionsBuilder.UseSnakeCaseNamingConvention();
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.UseLoggerFactory(_loggeFactory);

            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("files");

            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(FilesWriteDbContext).Assembly,
                type => type.FullName?.Contains("Database.Configurations.Read") ?? false);
        }
    }
}
