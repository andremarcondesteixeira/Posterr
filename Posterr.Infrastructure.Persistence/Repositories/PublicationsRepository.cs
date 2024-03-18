using Posterr.Core.Application.Interfaces;
using Posterr.Core.Domain.Boundaries.Persistence;
using System.Net;

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
        throw new NotImplementedException();
    }

    public Task<IList<IPublication>> Paginate(int lastSeenRow, ushort pageSize)
    {
        throw new NotImplementedException();
    }

    public Task<IPost> PublishNewPost(IUnpublishedPost unpublishedPost)
    {
        throw new NotImplementedException();
    }

    public Task<IRepost> PublishNewRepost(IUnpublishedRepost unpublishedRepost)
    {
        throw new NotImplementedException();
    }
}
