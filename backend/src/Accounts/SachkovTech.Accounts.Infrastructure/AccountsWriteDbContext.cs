using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SachkovTech.Accounts.Domain;

namespace SachkovTech.Accounts.Infrastructure;

public class AccountsWriteDbContext(IConfiguration configuration)
    : IdentityDbContext<User, Role, Guid>
{
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<AdminAccount> AdminAccounts => Set<AdminAccount>();
    public DbSet<RefreshSession> RefreshSessions => Set<RefreshSession>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(configuration.GetConnectionString("Database"));
        optionsBuilder.UseSnakeCaseNamingConvention();
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.UseLoggerFactory(CreateLoggerFactory());
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .ToTable("users");

        modelBuilder.Entity<User>()
            .HasMany(u => u.Roles)
            .WithMany()
            .UsingEntity<IdentityUserRole<Guid>>();

        modelBuilder.Entity<AdminAccount>()
            .HasOne(a => a.User)
            .WithOne()
            .HasForeignKey<AdminAccount>(a => a.UserId);

        modelBuilder.Entity<AdminAccount>()
            .ComplexProperty(a => a.FullName, fb =>
            {
                fb.Property(a => a.FirstName).IsRequired().HasColumnName("first_name");
                fb.Property(a => a.SecondName).IsRequired().HasColumnName("second_name");
            });

        modelBuilder.Entity<Role>()
            .ToTable("roles");

        modelBuilder.Entity<RefreshSession>()
            .ToTable("refresh_sessions");

        modelBuilder.Entity<RefreshSession>()
            .HasOne(r => r.User)
            .WithMany()
            .HasForeignKey(r => r.UserId);

        modelBuilder.Entity<IdentityUserClaim<Guid>>()
            .ToTable("user_claims");

        modelBuilder.Entity<IdentityUserToken<Guid>>()
            .ToTable("user_tokens");

        modelBuilder.Entity<IdentityUserLogin<Guid>>()
            .ToTable("user_logins");

        modelBuilder.Entity<IdentityRoleClaim<Guid>>()
            .ToTable("role_claims");

        modelBuilder.Entity<IdentityUserRole<Guid>>()
            .ToTable("user_roles");

        modelBuilder.Entity<RolePermission>()
            .ToTable("role_permissions");

        modelBuilder.Entity<Permission>()
            .ToTable("permissions");

        modelBuilder.Entity<Permission>()
            .HasIndex(p => p.Code)
            .IsUnique();

        modelBuilder.Entity<RolePermission>()
            .HasKey(rp => new { rp.RoleId, rp.PermissionId });

        modelBuilder.Entity<RolePermission>()
            .HasOne(rp => rp.Role)
            .WithMany(r => r.RolePermissions)
            .HasForeignKey(rp => rp.RoleId);

        modelBuilder.Entity<RolePermission>()
            .HasOne(rp => rp.Permission)
            .WithMany()
            .HasForeignKey(rp => rp.PermissionId);

        modelBuilder.HasDefaultSchema("accounts");
    }

    private ILoggerFactory CreateLoggerFactory() =>
        LoggerFactory.Create(builder => { builder.AddConsole(); });
}