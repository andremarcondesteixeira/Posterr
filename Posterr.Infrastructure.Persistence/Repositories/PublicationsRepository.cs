using Posterr.Core.Application.Interfaces;
using Posterr.Core.Domain.Boundaries.Persistence;
using Posterr.Infrastructure.Persistence.DbEntities;

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

    public Task<IList<IPublication>> Paginate(int lastSeenRow, ushort pageSize)
    {
        throw new NotImplementedException();
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
}
