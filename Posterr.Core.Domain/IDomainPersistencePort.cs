using Posterr.Core.Domain.Publications;
using Posterr.Core.Domain.Users;

namespace Posterr.Core.Domain;

// I decided to make the interface methods async because it will make the
// interface compatible with a wider range of persistence methods
public interface IDomainPersistencePort
{
    Task<ushort> AmountOfPublicationsMadeTodayBy(User author);
    Task<Post> PublishNewPost(UnpublishedPost unpublishedPost);
}
