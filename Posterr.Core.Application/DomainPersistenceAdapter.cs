using Posterr.Core.Application.Boundaries.Persistence;
using Posterr.Core.Domain;
using Posterr.Core.Domain.Boundaries.Persistence;

namespace Posterr.Core.Application;

public sealed class DomainPersistenceAdapter(IPublicationsRepository publicationsRepository) : IDomainPersistencePort
{
    public Task<ushort> AmountOfPublicationsMadeTodayBy(IUser author) =>
        publicationsRepository.CountPublicationsByUserBetween(author,
                                                              DateTime.UtcNow.Date,
                                                              DateTime.UtcNow.Date.AddDays(1).AddTicks(-1));

    public Task<IPost> PublishNewPost(IUnpublishedPost unpublishedPost) =>
        publicationsRepository.PublishNewPost(unpublishedPost);

    public Task<IRepost> PublishNewRepost(IUnpublishedRepost unpublishedRepost) =>
        publicationsRepository.PublishNewRepost(unpublishedRepost);
}
