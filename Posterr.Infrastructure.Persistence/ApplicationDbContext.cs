using Microsoft.EntityFrameworkCore;
using Posterr.Infrastructure.Persistence.DbEntities;

namespace Posterr.Infrastructure.Persistence;

public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<PostDbEntity> Posts { get; set; }
    public DbSet<RepostDbEntity> Reposts { get; set; }
    public DbSet<UserDbEntity> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder ?? throw new ArgumentNullException(nameof(modelBuilder)));

        modelBuilder.Entity<UserDbEntity>()
                    .Property(nameof(UserDbEntity.Username))
                    .HasColumnType("VARCHAR(20)");

        modelBuilder.Entity<UserDbEntity>().HasData(
            new UserDbEntity { Id = 1, CreatedAt = DateTime.UtcNow, Username = "simba" },
            new UserDbEntity { Id = 2, CreatedAt = DateTime.UtcNow, Username = "nala" },
            new UserDbEntity { Id = 3, CreatedAt = DateTime.UtcNow, Username = "timon" },
            new UserDbEntity { Id = 4, CreatedAt = DateTime.UtcNow, Username = "pumbaa" }
        );

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (entityType.ClrType.IsSubclassOf(typeof(BaseDbEntity)))
            {
                modelBuilder.Entity(entityType.ClrType)
                            .Property(nameof(BaseDbEntity.CreatedAt))
                            .HasDefaultValueSql("GETDATE()");
            }
        }

        modelBuilder.Entity<PostDbEntity>()
                    .HasOne(post => post.User)
                    .WithMany()
                    .HasForeignKey(x => x.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<RepostDbEntity>()
                    .Property(nameof(RepostDbEntity.CreatedAt))
                    .HasDefaultValueSql("GETDATE()");

        modelBuilder.Entity<RepostDbEntity>()
                    .HasOne(repost => repost.User)
                    .WithMany()
                    .HasForeignKey(x => x.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<RepostDbEntity>()
                    .HasOne(repost => repost.Post)
                    .WithMany()
                    .HasForeignKey(x => x.PostId)
                    .OnDelete(DeleteBehavior.Restrict);
    }
}
