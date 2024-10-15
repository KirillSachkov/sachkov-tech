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
        modelBuilder.Entity<User>()
            .ToTable("user");
        
        modelBuilder.Entity<Role>()
            .ToTable("role");
        
        modelBuilder.Entity<Notifications>()
            .ToTable("notifications");
        
        modelBuilder.Entity<Notifications>()
            .HasKey(x => x.Id);

        modelBuilder.Entity<Notifications>()
            .HasMany(n => n.Users)
            .WithOne();
        
        modelBuilder.Entity<Notifications>()
            .HasMany(n => n.Roles)
            .WithOne();
        
        modelBuilder.Entity<NotificationSettings>()
            .ToTable("notification_settings");
        
        modelBuilder.Entity<NotificationSettings>()
            .HasKey(n => n.Id);

        modelBuilder.Entity<NotificationSettings>()
            .HasOne<User>()
            .WithOne();
    }
}