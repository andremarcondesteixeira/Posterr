
using Posterr.Core.Domain.Boundaries.Persistence;

namespace Posterr.Core.Application.Interfaces;

public interface IPublicationsRepository
{
    Task<ushort> CountPublicationsByUser(IUser author);
    Task<IPost?> FindPostById(long originalPostId);
    Task<IList<IPublication>> Paginate(int lastSeenRow, ushort pageSize);
    Task<IPost> PublishNewPost(IUnpublishedPost unpublishedPost);
    Task<IRepost> PublishNewRepost(IUnpublishedRepost unpublishedRepost);
}
