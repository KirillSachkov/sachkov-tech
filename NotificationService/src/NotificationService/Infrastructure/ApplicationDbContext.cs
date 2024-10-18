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
        modelBuilder.Entity<Notification>()
            .ToTable("notifications");

        modelBuilder.Entity<Notification>()
            .HasKey(x => x.Id);

        modelBuilder.Entity<Notification>()
            .Property(n => n.UserIds)
            .HasColumnType("uuid[]");

        modelBuilder.Entity<Notification>()
            .Property(n => n.RoleIds)
            .HasColumnType("uuid[]");

        modelBuilder.Entity<Notification>()
            .Property(n => n.Message)
            .HasMaxLength(5000);

        modelBuilder.Entity<NotificationSettings>()
            .ToTable("notification_settings");

        modelBuilder.Entity<NotificationSettings>()
            .HasKey(x => x.Id);
    }
}