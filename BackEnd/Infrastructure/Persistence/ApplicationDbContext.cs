using Microsoft.EntityFrameworkCore;
using Posterr.Infrastructure.Persistence.DbEntities;
using System.Globalization;
using System.Reflection;

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

        Dictionary<string, long> authorIds = new() {
            { "Mufasa", 1 },
            { "Mufasa (spirit)", 2 },
            { "Sarabi", 3 },
            { "Scar", 4 },
            { "Rafiki", 5 },
            { "Zazu", 6 },
            { "Simba (cub)", 7 },
            { "Simba", 8 },
            { "Nala (cub)", 9 },
            { "Nala", 10 },
            { "Timon", 11 },
            { "Pumbaa", 12 },
            { "Shenzi", 13 },
            { "Kamari", 14 },
            { "Singers", 15 }
        };

        List<UserDbEntity> initialUsers = authorIds.Select((keyValuePair) => new UserDbEntity
        {
            Id = keyValuePair.Value,
            Username = keyValuePair.Key
        }).ToList();

        List<PublicationDbEntity> initialPosts = [
            new PublicationDbEntity {
                Id = 1,
                AuthorId = 15,
                AuthorUsername = "Singers",
                PublicationDate = new DateTime(1994, 7, 9, 12, 0, 0, DateTimeKind.Utc),
                Content = """
                    Nants ingonyama bagithi Baba
                    (Sithi uhm ingonyama)
                    Nants ingonyama bagithi baba
                    (Sithi uhhmm ingonyama, Ingonyama)
                    Siyo Nqoba
                    (Ingonyama)
                    Ingonyama nengw' enamabala
                    Ingonyama nengw' enamabala
                    Ingonyama nengw' enamabala
                    Ingonyama nengw' enamabala
                """,
            }
        ];

        var assembly = Assembly.GetExecutingAssembly();
        string postsSeedResourceName = assembly.GetManifestResourceNames().Single(str => str.EndsWith("posts.csv"));
        Stream resourceStream = assembly.GetManifestResourceStream(postsSeedResourceName)!;

        using (StreamReader reader = new(resourceStream))
        {
            reader.ReadLine();
            long postId = 1;
            while (reader.ReadLine() is string line)
            {
                string[] parts = line.Split(';');

                var when = DateTime.Parse(parts[0], CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal).ToUniversalTime();
                string username = parts[1];
                string post = parts[2];

                initialPosts.Add(new PublicationDbEntity
                {
                    Id = ++postId,
                    AuthorId = authorIds[username],
                    AuthorUsername = username,
                    PublicationDate = when,
                    Content = post
                });
            }
        }

        modelBuilder.Entity<UserDbEntity>().HasData(initialUsers);
        modelBuilder.Entity<PublicationDbEntity>().HasData(initialPosts);
    }
}
