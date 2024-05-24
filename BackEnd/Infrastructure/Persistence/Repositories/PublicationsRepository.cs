using Microsoft.EntityFrameworkCore;
using Posterr.Core.Boundaries.Persistence;
using Posterr.Core.Boundaries.EntitiesInterfaces;
using Posterr.Infrastructure.Persistence.DbEntities;
using System.Data.Common;

namespace Posterr.Infrastructure.Persistence.Repositories;

public class PublicationsRepository(ApplicationDbContext dbContext) : IPublicationsRepository
{
    public Task<ushort> CountPublicationsByUserBetween(IUser author, DateTime startInclusive, DateTime endInclusive)
    {
        var count = (ushort) (
            from result in
                (
                    from post in dbContext.Posts
                    join user in dbContext.Users on post.UserId equals user.Id
                    where user.Username == author.Username
                          && post.CreatedAt >= startInclusive
                          && post.CreatedAt <= endInclusive
                    group post by user.Username into posts
                    select new { Amount = posts.Count(), Of = "posts" }
                ).Union
                (
                    from repost in dbContext.Reposts
                    join user in dbContext.Users on repost.UserId equals user.Id
                    where user.Username == author.Username
                          && repost.CreatedAt >= startInclusive
                          && repost.CreatedAt <= endInclusive
                    group repost by user.Username into reposts
                    select new { Amount = reposts.Count(), Of = "reposts" }
                )
            group result by result.Amount into amounts
            select amounts.Sum(result => result.Amount)
        ).First();

        return Task.FromResult(count);
    }

    public Task<IPost?> FindPostById(long originalPostId)
    {
        var postDbEntity = dbContext.Posts.Where(post => post.Id == originalPostId).First();

        if (postDbEntity is null)
        {
            return Task.FromResult<IPost?>(null);
        }

        return Task.FromResult<IPost?>(postDbEntity.ToIPost());
    }

    public Task<IPost> PublishNewPost(IUnpublishedPost unpublishedPost)
    {
        var user = dbContext.Users.Where(user => user.Username == unpublishedPost.Author.Username).First()
            ?? throw new NullReferenceException($"No User was found with username \"{unpublishedPost.Author.Username}\"");
        
        var postDbEntity = new PostDbEntity()
        {
            UserId = user.Id,
            User = user,
            Content = unpublishedPost.Content
        };
        
        var postDbEntry = dbContext.Posts.Add(postDbEntity);
        dbContext.SaveChanges();

        return Task.FromResult(postDbEntity.ToIPost());

    }

    public Task<IRepost> PublishNewRepost(IUnpublishedRepost unpublishedRepost)
    {
        var user = dbContext.Users.Where(user => user.Username == unpublishedRepost.Author.Username).First()
            ?? throw new NullReferenceException($"No user was found with username \"{unpublishedRepost.Author.Username}\"");

        var post = dbContext.Posts.Find(unpublishedRepost.OriginalPost.Id)
            ?? throw new NullReferenceException($"No post was found with id {unpublishedRepost.OriginalPost.Id}");

        var repostDbEntity = new RepostDbEntity()
        {
            User = user,
            UserId = user.Id,
            Post = post,
            PostId = post.Id,
        };

        var repostDbEntry = dbContext.Reposts.Add(repostDbEntity);
        dbContext.SaveChanges();

        return Task.FromResult(repostDbEntity.ToIRepost());
    }

    public Task<IList<IPublication>> Paginate(int lastSeenRow, short pageSize)
    {
        using DbCommand command = dbContext.Database.GetDbConnection().CreateCommand();
        command.CommandText = POSTS_PAGINATION_QUERY;
        AddQueryParams(lastSeenRow, pageSize, command);
        dbContext.Database.OpenConnection();
        using DbDataReader reader = command.ExecuteReader();
        IList<IPublication> posts = GetPostsList(reader);
        dbContext.Database.CloseConnection();
        return Task.FromResult(posts);
    }

    private const string POSTS_PAGINATION_QUERY = @"
            SELECT * FROM (
                SELECT
                    Combined.PostId,
                    Combined.PostUserId,
                    UsersPost.Username AS PostUsername,
                    UsersPost.CreatedAt AS PostUserCreatedAt,
                    Combined.PostContent,
                    Combined.PostCreatedAt,
                    Combined.IsRepost,
                    Combined.RepostUserId,
                    UsersRepost.Username AS RepostUsername,
                    UsersRepost.CreatedAt as RepostUserCreatedAt,
                    Combined.RepostCreatedAt,
                    Combined.PublicationCreatedAt,
                    ROW_NUMBER() OVER (ORDER BY Combined.PublicationCreatedAt DESC) AS RowNumber
                FROM (
                    SELECT
                        Id AS PostId,
                        UserId AS PostUserId,
                        Content AS PostContent, 
                        CreatedAt AS PostCreatedAt,
                        CAST(0 AS BIT) AS IsRepost,
                        NULL AS RepostUserId,
                        NULL AS RepostCreatedAt,
                        CreatedAt AS PublicationCreatedAt
                    FROM Posts
    
                    UNION ALL
    
                    SELECT
                        p.Id AS PostId,
                        p.UserId AS PostUserId,
                        p.Content AS PostContent,
                        p.CreatedAt AS PostCreatedAt,
                        CAST(1 AS BIT) AS IsRepost,
                        r.UserId AS RepostUserId,
                        r.CreatedAt AS RepostCreatedAt,
                        r.CreatedAt AS PublicationCreatedAt
                    FROM Reposts AS r
                    INNER JOIN Posts AS p ON p.Id = r.PostId
                ) AS Combined
                LEFT JOIN Users AS UsersPost ON Combined.PostUserId = UsersPost.Id
                LEFT JOIN Users AS UsersRepost ON Combined.RepostUserId = UsersRepost.Id
            ) AS SubQuery
            WHERE RowNumber > @lastSeenRowNumber
            ORDER BY PublicationCreatedAt DESC
            OFFSET 0 ROWS FETCH FIRST @pageSize ROWS ONLY;
    ";

    private static void AddQueryParams(int lastSeenRowNumber, short pageSize, DbCommand command)
    {
        DbParameter lastSeenRowNumberParam = command.CreateParameter();
        lastSeenRowNumberParam.ParameterName = "@lastSeenRowNumber";
        lastSeenRowNumberParam.Value = lastSeenRowNumber;
        command.Parameters.Add(lastSeenRowNumberParam);

        DbParameter pageSizeParam = command.CreateParameter();
        pageSizeParam.ParameterName = "@pageSize";
        pageSizeParam.Value = pageSize;
        command.Parameters.Add(pageSizeParam);
    }

    private static List<IPublication> GetPostsList(DbDataReader reader)
    {
        List<IPublication> contents = [];

        while (reader.Read())
        {
            PostColumns postColumns = GetPostColumns(reader);
            UserDbEntity postAuthor = new()
            {
                Id = postColumns.PostUserId,
                Username = postColumns.PostUsername,
                CreatedAt = postColumns.PostUserCreatedAt
            };
            PostDbEntity post = new()
            {
                Id = postColumns.PostId,
                User = postAuthor,
                UserId = postAuthor.Id,
                Content = postColumns.PostContent,
                CreatedAt = postColumns.PostCreatedAt
            };

            if (GetRepostColumns(reader) is RepostColumns repostFields)
            {
                UserDbEntity repostAuthor = new()
                {
                    Id = repostFields.RepostUserId,
                    Username = repostFields.RepostUsername,
                    CreatedAt = repostFields.RepostUserCreatedAt
                };
                contents.Add(
                    new RepostDbEntity()
                    {
                        User = repostAuthor,
                        UserId = repostAuthor.Id,
                        Post = post,
                        PostId = post.Id,
                        CreatedAt = repostFields.RepostCreatedAt
                    }.ToIRepost()
                );
            }
            else
            {
                contents.Add(post.ToIPost());
            }
        }

        return contents;
    }

    private static PostColumns GetPostColumns(DbDataReader reader)
    {
        return new PostColumns
        {
            PostId = reader.GetInt64(0),
            PostUserId = reader.GetInt64(1),
            PostUsername = reader.GetString(2),
            PostUserCreatedAt = reader.GetDateTime(3),
            PostContent = reader.GetString(4),
            PostCreatedAt = reader.GetDateTime(5)
        };
    }

    private static RepostColumns? GetRepostColumns(DbDataReader reader)
    {
        bool isRepost = reader.GetBoolean(6);
        long? repostUserId = reader.IsDBNull(7) ? null : reader.GetInt64(7);
        string? repostUsername = reader.IsDBNull(8) ? null : reader.GetString(8);
        DateTime? repostUserCreatedAt = reader.IsDBNull(9) ? null : reader.GetDateTime(9);
        DateTime? repostCreatedAt = reader.IsDBNull(10) ? null : reader.GetDateTime(10);

        if (isRepost && repostUserId != null && repostUsername != null && repostUserCreatedAt != null && repostCreatedAt != null)
        {
            return new RepostColumns
            {
                RepostUsername = repostUsername,
                RepostCreatedAt = (DateTime)repostCreatedAt,
                RepostUserCreatedAt = (DateTime)repostUserCreatedAt,
                RepostUserId = (long)repostUserId
            };
        }

        return null;
    }

    internal readonly struct PostColumns
    {
        public required readonly long PostId { get; init; }
        public required readonly long PostUserId { get; init; }
        public required readonly string PostUsername { get; init; }
        public required readonly DateTime PostUserCreatedAt { get; init; }
        public required readonly string PostContent { get; init; }
        public required readonly DateTime PostCreatedAt { get; init; }
    }

    internal readonly struct RepostColumns
    {
        public required readonly long RepostUserId { get; init; }
        public required readonly string RepostUsername { get; init; }
        public required readonly DateTime RepostUserCreatedAt { get; init; }
        public required readonly DateTime RepostCreatedAt { get; init; }
    };
}
