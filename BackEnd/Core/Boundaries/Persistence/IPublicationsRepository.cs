using Posterr.Core.Boundaries.EntitiesInterfaces;

namespace Posterr.Core.Boundaries.Persistence;

public interface IPublicationsRepository
{
    IPublication? FindById(long publicationId);
    int CountPublicationsMadeByUserBetweenDateTimeRange(IUser author, DateTime startInclusive, DateTime endInclusive);
    IList<IPublication> Paginate(long lastSeenPublicationId, short pageSize);
    IPost PublishNewPost(IUnpublishedPost unpublishedPost);
    IRepost PublishNewRepost(IUnpublishedRepost unpublishedRepost);
}
