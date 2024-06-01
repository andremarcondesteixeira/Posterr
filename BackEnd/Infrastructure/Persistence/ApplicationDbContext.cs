using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Posterr.Infrastructure.Persistence.DbEntities;

namespace Posterr.Infrastructure.Persistence;

public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<PublicationDbEntity> Publications { get; set; }
    public DbSet<UserDbEntity> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder ?? throw new ArgumentNullException(nameof(modelBuilder)));

        modelBuilder.Entity<UserDbEntity>()
                    .Property(nameof(UserDbEntity.Username))
                    .HasColumnType("VARCHAR(20)");

        modelBuilder.Entity<UserDbEntity>().HasData(
            new UserDbEntity { Id = 1, Username = "simba" },
            new UserDbEntity { Id = 2, Username = "nala" },
            new UserDbEntity { Id = 3, Username = "timon" },
            new UserDbEntity { Id = 4, Username = "pumbaa" }
        );

        modelBuilder.Entity<PublicationDbEntity>()
                    .HasOne(publication => publication.Author)
                    .WithMany()
                    .HasForeignKey(x => x.AuthorId)
                    .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<PublicationDbEntity>()
                    .HasOne(publication => publication.OriginalPost)
                    .WithMany()
                    .HasForeignKey(x => x.OriginalPostId)
                    .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<PublicationDbEntity>()
                    .Property(nameof(PublicationDbEntity.PublicationDate))
                    .HasDefaultValueSql("NOW()");
    }
}
