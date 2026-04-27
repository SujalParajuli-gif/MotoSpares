using Microsoft.EntityFrameworkCore;
using MotoSpares.Domain.Entities;

namespace MotoSpares.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Staff> Staff { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User configuration
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.HasIndex(u => u.Email).IsUnique();
            entity.Property(u => u.FullName).IsRequired().HasMaxLength(100);
            entity.Property(u => u.Email).IsRequired().HasMaxLength(100);
            entity.Property(u => u.PasswordHash).IsRequired();
            entity.Property(u => u.Phone).HasMaxLength(20);
            entity.Property(u => u.Role).HasConversion<string>();
        });

        // Staff configuration
        modelBuilder.Entity<Staff>(entity =>
        {
            entity.HasKey(s => s.Id);
            entity.Property(s => s.EmployeeCode).IsRequired().HasMaxLength(50);
            entity.Property(s => s.Department).IsRequired().HasMaxLength(100);

            // Staff → User relationship
            entity.HasOne(s => s.User)
                  .WithOne()
                  .HasForeignKey<Staff>(s => s.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Seed Admin
        var adminId = Guid.NewGuid();
        modelBuilder.Entity<User>().HasData(new
        {
            Id = adminId,
            FullName = "Admin",
            Email = "admin@motospares.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
            Role = "Admin",
            Phone = "0000000000",
            CreatedAt = DateTime.UtcNow
        });
    }
}