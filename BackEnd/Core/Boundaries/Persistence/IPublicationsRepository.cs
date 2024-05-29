using Posterr.Core.Boundaries.EntitiesInterfaces;

namespace Posterr.Core.Boundaries.Persistence;

public interface IPublicationsRepository
{
    Task<int> CountPublicationsByUserBetween(IUser author, DateTime startInclusive, DateTime endInclusive);
    Task<IPost?> FindPostById(long originalPostId);
    Task<IList<IPublication>> Paginate(int lastSeenRow, ushort pageSize);
    Task<IPost> PublishNewPost(IUnpublishedPost unpublishedPost);
    Task<IRepost> PublishNewRepost(IUnpublishedRepost unpublishedRepost);
}
