using Gardener.entities;
using Microsoft.EntityFrameworkCore;

namespace Gardener.Database;

public class GardenerDbContext(DbContextOptions<GardenerDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Plant> Plants { get; set; }
    public DbSet<Growth> Growths { get; set; }
    public DbSet<BioChar> BioChars { get; set; }
    public DbSet<Preferences> Preferences { get; set; }
    public DbSet<Favorite> Favorites { get; set; }
    public DbSet<PlantRating> PlantRatings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("grd");

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.HasIndex(u => u.Login).IsUnique();
            entity.HasIndex(u => u.Phone).IsUnique();
            entity.Property(u => u.Login).HasMaxLength(50);
            entity.Property(u => u.Password).HasMaxLength(100);
            entity.Property(u => u.NickName).HasMaxLength(30);
            entity.Property(u => u.Phone).HasMaxLength(15);
            entity.Property(u => u.Role).HasConversion<string>();
        });

        modelBuilder.Entity<Plant>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.HasIndex(p => p.ScienceName).IsUnique();
            entity.Property(p => p.Name).HasMaxLength(30);
            entity.Property(p => p.ScienceName).HasMaxLength(100);
            entity.Property(p => p.Description).HasMaxLength(8000);
            entity.Property(p => p.Photo).HasMaxLength(300);
        });

        modelBuilder.Entity<Growth>(entity =>
        {
            entity.HasKey(g => g.Id);
            entity.HasOne(g => g.Plant)
                .WithOne()
                .HasForeignKey<Growth>(g => g.Id);
            entity.Property(g => g.Ppfd).HasMaxLength(9);
            entity.Property(g => g.Soil).HasMaxLength(25);
            entity.Property(g => g.Survivability);
            entity.Property(g => g.GrowthSpeed).HasMaxLength(5);
            entity.Property(g => g.Climate).HasMaxLength(20);
            entity.Property(g => g.Water).HasMaxLength(20);
            entity.Property(g => g.LandScape).HasMaxLength(20);
        });

        modelBuilder.Entity<BioChar>(entity =>
        {
            entity.HasKey(b => b.Id);
            entity.HasOne(b => b.Plant)
                .WithOne()
                .HasForeignKey<BioChar>(b => b.Id);
            entity.Property(b => b.LeafType).HasMaxLength(25);
            entity.Property(b => b.Root).HasMaxLength(20);
            entity.Property(b => b.Fruit).HasMaxLength(20);
            entity.Property(b => b.AmmFruit).HasMaxLength(1);
            entity.Property(b => b.Morphology).HasMaxLength(30);
        });

        modelBuilder.Entity<Preferences>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.HasOne(p => p.User)
                .WithOne()
                .HasForeignKey<Preferences>(p => p.Id);
            entity.Property(p => p.Climate).HasMaxLength(20);
            entity.Property(p => p.Soil).HasMaxLength(20);
            entity.Property(p => p.Water).HasMaxLength(20);
            entity.Property(p => p.LandScape).HasMaxLength(20);
        });

        modelBuilder.Entity<Favorite>(entity =>
        {
            entity.HasKey(f => new { f.UserId, f.PlantId });
            entity.HasOne(f => f.User)
                    .WithMany()
                    .HasForeignKey(f => f.UserId);
            entity.HasOne(f => f.Plant)
                    .WithMany()
                    .HasForeignKey(f => f.PlantId);
        });

        modelBuilder.Entity<PlantRating>(entity =>
        {
            entity.HasKey(r => r.Id);
            entity.HasOne(r => r.Plant)
                .WithOne()
                .HasForeignKey<PlantRating>(r => r.Id);
            entity.Property(r => r.ViewCount).HasDefaultValue(0);
            entity.Property(r => r.FavoriteCount).HasDefaultValue(0);
        });
    }
}