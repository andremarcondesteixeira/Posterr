using Posterr.Core.Boundaries.EntitiesInterfaces;

namespace Posterr.Core.Boundaries.Persistence;

public interface IPublicationsRepository
{
    int CountPublicationsMadeByUserBetweenDateTimeRange(IUser author, DateTime startInclusive, DateTime endInclusive);
    int CountRepostsByUserAndOriginalPost(IUser author, IPost originalPost);
    IPublication? FindById(long publicationId);
    IList<IPublication> GetNMostRecentPublications(short pageSize);
    IList<IPublication> Paginate(long lastSeenPublicationId, short pageSize);
    IPost PublishNewPost(IUnpublishedPost unpublishedPost);
    IRepost PublishNewRepost(IUnpublishedRepost unpublishedRepost);
}
