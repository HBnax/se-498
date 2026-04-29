using Microsoft.EntityFrameworkCore;
using pitchamon.Backend.Models;

namespace pitchamon.Backend.Data;

public class BackendDbContext : DbContext
{
    public BackendDbContext(DbContextOptions<BackendDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<ProcessingHistory> ProcessingHistory => Set<ProcessingHistory>();

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

        modelBuilder.Entity<ProcessingHistory>(entity =>
        {
            entity.ToTable("processing_history");

            entity.HasKey(p => p.Id);

            entity.Property(p => p.Id)
                .HasColumnName("id");

            entity.Property(p => p.UserId)
                .HasColumnName("user_id");

            entity.Property(p => p.OriginalSongFile)
                .HasColumnName("original_song_file")
                .IsRequired();

            entity.Property(p => p.PokemonUsed)
                .HasColumnName("pokemon_used")
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(p => p.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .ValueGeneratedOnAdd();
        });
    }
}