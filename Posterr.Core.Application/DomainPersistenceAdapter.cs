using Posterr.Core.Application.Interfaces;
using Posterr.Core.Domain;
using Posterr.Core.Domain.Boundaries.Persistence;

namespace Posterr.Core.Application;

public sealed class DomainPersistenceAdapter(IPublicationsRepository publicationsRepository) : IDomainPersistencePort
{
    public Task<ushort> AmountOfPublicationsMadeTodayBy(IUser author) =>
        publicationsRepository.CountPublicationsByUser(author);

    public Task<IPost> PublishNewPost(IUnpublishedPost unpublishedPost) =>
        publicationsRepository.PublishNewPost(unpublishedPost);

    public Task<IRepost> PublishNewRepost(IUnpublishedRepost unpublishedRepost) =>
        publicationsRepository.PublishNewRepost(unpublishedRepost);
}
