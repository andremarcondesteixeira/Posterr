using Posterr.Core.Shared.EntitiesInterfaces;
using Posterr.Core.Shared.Enums;

namespace Posterr.Core.Shared.PersistenceInterfaces;

public interface IPublicationsRepository
{
    int CountPublicationsMadeByUserBetweenDateTimeRange(IUser author, DateTime startInclusive, DateTime endInclusive);
    int CountRepostsByUserAndOriginalPost(IUser author, IPost originalPost);
    IPublication? FindById(long publicationId);
    IList<IPublication> Paginate(bool isFirstPage, long lastSeenPublicationId, short pageSize, SortOrder sortOrder);
    IPost PublishNewPost(IUnpublishedPost unpublishedPost);
    IRepost PublishNewRepost(IUnpublishedRepost unpublishedRepost);
    IList<IPublication> Search(string searchTerm);
}
