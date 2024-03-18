using Posterr.Core.Domain.PersistenceBoundaryInterfaces;

namespace Posterr.Core.Domain;

// I decided to make the interface methods async because it will make the
// interface compatible with a wider range of persistence methods
public interface IDomainPersistencePort
{
    Task<ushort> AmountOfPublicationsMadeTodayBy(IUser author);
    Task<IPost> PublishNewPost(IUnpublishedPost unpublishedPost);
    Task<IRepost> PublishNewRepost(IUnpublishedRepost unpublishedRepost);
}
