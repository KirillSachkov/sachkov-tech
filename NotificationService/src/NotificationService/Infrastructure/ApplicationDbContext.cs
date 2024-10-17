using Microsoft.EntityFrameworkCore;
using NotificationService.Entities;

namespace NotificationService.Infrastructure;

public class ApplicationDbContext(IConfiguration configuration) : DbContext
{
    private const string DATABASE = "Database";
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(configuration.GetConnectionString(DATABASE));
        
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Notifications>()
            .ToTable("notifications");

        modelBuilder.Entity<Notifications>()
            .HasKey(x => x.Id);

        modelBuilder.Entity<Notifications>()
            .Property(n => n.UserIds)
            .HasColumnType("uuid[]");

        modelBuilder.Entity<Notifications>()
            .Property(n => n.RoleIds)
            .HasColumnType("uuid[]");
        
        modelBuilder.Entity<NotificationSettings>()
            .ToTable("notification_settings");
        
        modelBuilder.Entity<NotificationSettings>()
            .HasKey(x => x.Id);
    }
}