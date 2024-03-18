using Posterr.Core.Application.Interfaces;
using Posterr.Core.Domain.Boundaries.Persistence;

namespace Posterr.Infrastructure.Persistence.Repositories;

public class PublicationsRepository : IPublicationsRepository
{
    public Task<ushort> CountPublicationsByUser(IUser author)
    {
        throw new NotImplementedException();
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
