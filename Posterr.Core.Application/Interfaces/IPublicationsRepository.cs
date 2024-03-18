
using Posterr.Core.Domain.Boundaries.Persistence;

namespace Posterr.Core.Application.Interfaces;

public interface IPublicationsRepository
{
    Task<ushort> CountPublicationsByUserBetween(IUser author, DateTime startInclusive, DateTime endInclusive);
    Task<IPost?> FindPostById(long originalPostId);
    Task<IList<IPublication>> Paginate(int lastSeenRow, short pageSize);
    Task<IPost> PublishNewPost(IUnpublishedPost unpublishedPost);
    Task<IRepost> PublishNewRepost(IUnpublishedRepost unpublishedRepost);
}
