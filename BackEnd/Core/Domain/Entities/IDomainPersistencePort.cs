using Posterr.Core.Boundaries.EntitiesInterfaces;

namespace Posterr.Core.Domain.Entities;

// I decided to make the interface methods async because it will make the
// interface compatible with a wider range of persistence methods.
//
// This interface is not in the Boundaries project because putting it in the Boundaries project
// would allow the persistence layer to have knowledge about the domain layer
public interface IDomainPersistencePort
{
    Task<int> AmountOfPublicationsMadeTodayBy(IUser author);
    Task<IPost> PublishNewPost(IUnpublishedPost unpublishedPost);
    Task<IRepost> PublishNewRepost(IUnpublishedRepost unpublishedRepost);
}
