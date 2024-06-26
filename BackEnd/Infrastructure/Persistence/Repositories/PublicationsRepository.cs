#pragma warning disable IDE0305 // Simplify collection initialization

using Microsoft.EntityFrameworkCore;
using Posterr.Core.Shared.EntitiesInterfaces;
using Posterr.Core.Shared.Enums;
using Posterr.Core.Shared.Exceptions;
using Posterr.Core.Shared.PersistenceInterfaces;
using Posterr.Infrastructure.Persistence.DbEntities;
using System.Data;
using System.Data.Common;

namespace Posterr.Infrastructure.Persistence.Repositories;

public class PublicationsRepository(ApplicationDbContext dbContext) : IPublicationsRepository
{
    public int CountPublicationsMadeByUserBetweenDateTimeRange(IUser author, DateTime startInclusive, DateTime endInclusive)
    {
        return dbContext.Publications.Where(p =>
            p.AuthorUsername == author.Username &&
            p.PublicationDate >= startInclusive &&
            p.PublicationDate <= endInclusive
        ).Count();
    }

    public int CountRepostsByUserAndOriginalPost(IUser author, IPost originalPost)
    {
        return dbContext.Publications.Where(p =>
            p.AuthorUsername == author.Username &&
            p.OriginalPostId == originalPost.Id
        ).Count();
    }

    public IPublication? FindById(long publicationId)
    {
        var queryResult = dbContext
            .Publications
            .Where(publication => publication.Id == publicationId)
            .Include(publication => publication.Author);

        if (!queryResult.Any())
        {
            return null;
        }

        PublicationDbEntity publication = queryResult.First();

        if (publication.OriginalPostId is null)
        {
            return publication.ToIPost();
        }

        return publication.ToIRepost();
    }

    public IList<IPublication> Paginate(bool isFirstPage, long lastSeenPublicationId, short pageSize, SortOrder sortOrder)
    {
        using DbCommand command = dbContext.Database.GetDbConnection().CreateCommand();
        command.CommandText = """
            SELECT
                "Id",
                "AuthorId",
                "AuthorUsername",
                "Content",
                "PublicationDate",
                "AmountOfReposts",
                "OriginalPostId",
                "OriginalPostAuthorId",
                "OriginalPostAuthorUsername",
                "OriginalPostContent",
                "OriginalPostPublicationDate"
            FROM "Publications"
            WHERE 1 = @isFirstPage OR "Id" < @lastSeenPublicationId
            ORDER BY
        """ + (
            sortOrder == SortOrder.NEWEST ?
                " \"PublicationDate\" DESC" :
                " \"AmountOfReposts\" DESC, \"PublicationDate\" DESC"
        ) + """
            OFFSET 0 ROWS FETCH FIRST @pageSize ROWS ONLY;
        """;

        DbParameter isFirstPageParam = command.CreateParameter();
        isFirstPageParam.ParameterName = "@isFirstPage";
        isFirstPageParam.Value = isFirstPage ? 1 : 0;
        command.Parameters.Add(isFirstPageParam);

        DbParameter lastSeenPublicationIdParam = command.CreateParameter();
        lastSeenPublicationIdParam.ParameterName = "@lastSeenPublicationId";
        lastSeenPublicationIdParam.Value = lastSeenPublicationId;
        command.Parameters.Add(lastSeenPublicationIdParam);

        DbParameter pageSizeParam = command.CreateParameter();
        pageSizeParam.ParameterName = "@pageSize";
        pageSizeParam.Value = pageSize;
        command.Parameters.Add(pageSizeParam);

        dbContext.Database.OpenConnection();
        using DbDataReader reader = command.ExecuteReader();

        List<IPublication> publications = [];

        while (reader.Read())
        {
            PublicationDbEntity publication = new()
            {
                Id = reader.GetInt64(0),
                AuthorId = reader.GetInt64(1),
                AuthorUsername = reader.GetString(2),
                Content = reader.GetString(3),
                PublicationDate = reader.GetDateTime(4),
                AmountOfReposts = reader.GetInt32(5),
                OriginalPostId = reader.IsDBNull(6) ? null : reader.GetInt64(6),
                OriginalPostAuthorId = reader.IsDBNull(7) ? null : reader.GetInt64(7),
                OriginalPostAuthorUsername = reader.IsDBNull(8) ? null : reader.GetString(8),
                OriginalPostContent = reader.IsDBNull(9) ? null : reader.GetString(9),
                OriginalPostPublicationDate = reader.IsDBNull(10) ? null : reader.GetDateTime(10),
            };

            if (publication.OriginalPostId == null)
            {
                publications.Add(publication.ToIPost());
            }
            else
            {
                publications.Add(publication.ToIRepost());
            }
        }

        dbContext.Database.CloseConnection();

        return publications;
    }

    public IPost PublishNewPost(IUnpublishedPost unpublishedPost)
    {
        var postDbEntity = new PublicationDbEntity()
        {
            AuthorId = unpublishedPost.Author.Id,
            AuthorUsername = unpublishedPost.Author.Username,
            Content = unpublishedPost.Content,
            PublicationDate = DateTime.UtcNow,
        };

        dbContext.Publications.Add(postDbEntity);
        dbContext.SaveChanges();

        return postDbEntity.ToIPost();
    }

    public IRepost PublishNewRepost(IUnpublishedRepost unpublishedRepost)
    {
        var repostUserQueryResult = dbContext
            .Users
            .Where(user => user.Username == unpublishedRepost.Author.Username);

        if (!repostUserQueryResult.Any())
        {
            throw new UserNotFoundException(unpublishedRepost.Author.Username);
        }

        UserDbEntity repostUser = repostUserQueryResult.First();

        var originalPostQuery = dbContext
            .Publications
            .Where(p => p.Id == unpublishedRepost.OriginalPost.Id)
            .Include(p => p.Author);

        if (!originalPostQuery.Any())
        {
            throw new PostNotFoundException(unpublishedRepost.OriginalPost.Id);
        }

        PublicationDbEntity originalPost = originalPostQuery.First()!;

        originalPost.AmountOfReposts++;

        var publicationDbEntity = new PublicationDbEntity()
        {
            Author = repostUser,
            AuthorId = repostUser.Id,
            AuthorUsername = repostUser.Username,
            Content = unpublishedRepost.Content,
            PublicationDate = DateTime.UtcNow,
            OriginalPost = originalPost,
            OriginalPostId = originalPost.Id,
            OriginalPostAuthorId = originalPost.AuthorId,
            OriginalPostAuthorUsername = originalPost.AuthorUsername,
            OriginalPostContent = originalPost.Content,
            OriginalPostPublicationDate = originalPost.PublicationDate,
        };

        var repostDbEntry = dbContext.Publications.Add(publicationDbEntity);

        dbContext.SaveChanges();

        return publicationDbEntity.ToIRepost();
    }

    public IList<IPublication> Search(string searchTerm)
    {
        return dbContext
            .Publications
            .Include(p => p.Author)
            .OrderByDescending(p => p.PublicationDate)
            .Where(p => p.OriginalPostId == null && p.Content.Contains(searchTerm))
            .Select(p => p.ToIPublication())
            .ToList();
    }
}
