using Posterr.Core.Shared.EntitiesInterfaces;

namespace Posterr.Core.Shared.PersistenceInterfaces;

public interface IPublicationsRepository
{
    int CountPublicationsMadeByUserBetweenDateTimeRange(IUser author, DateTime startInclusive, DateTime endInclusive);
    int CountRepostsByUserAndOriginalPost(IUser author, IPost originalPost);
    IPublication? FindById(long publicationId);
    IList<IPublication> GetNMostRecentPublications(short pageSize);
    IList<IPublication> Paginate(long lastSeenPublicationId, short pageSize);
    IPost PublishNewPost(IUnpublishedPost unpublishedPost);
    IRepost PublishNewRepost(IUnpublishedRepost unpublishedRepost);
    IList<IPublication> Search(string searchTerm, bool isFirstPage, short pageSize, long lastSeenPublicationId);
}
