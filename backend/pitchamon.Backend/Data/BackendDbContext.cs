using Microsoft.EntityFrameworkCore;
using pitchamon.Backend.Models;

namespace pitchamon.Backend.Data;

public class BackendDbContext : DbContext
{
    public BackendDbContext(DbContextOptions<BackendDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");

            entity.HasKey(u => u.Id);

            entity.Property(u => u.Id)
                .HasColumnName("id");

            entity.Property(u => u.Email)
                .HasColumnName("email")
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(u => u.PasswordHash)
                .HasColumnName("password_hash")
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(u => u.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .ValueGeneratedOnAdd();
        });
    }
}