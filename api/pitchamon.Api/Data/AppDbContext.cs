using Microsoft.EntityFrameworkCore;
using pitchamon.Api.Models;

namespace pitchamon.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Pokemon> Pokemon => Set<Pokemon>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Pokemon>(entity =>
        {
            entity.ToTable("pokemon");

            entity.HasKey(p => p.Id);

            entity.Property(p => p.Id)
                .HasColumnName("id")
                .ValueGeneratedNever();

            entity.Property(p => p.Name)
                .HasColumnName("name")
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(p => p.Cry)
                .HasColumnName("cry")
                .IsRequired()
                .HasMaxLength(255);

            entity.HasIndex(p => p.Name).IsUnique();
        });
    }
}