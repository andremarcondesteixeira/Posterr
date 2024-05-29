using Posterr.Core.Boundaries.Persistence;
using Posterr.Core.Boundaries.EntitiesInterfaces;
using Posterr.Core.Domain.Entities;

namespace Posterr.Core.Application.UseCases;

public sealed class DomainPersistenceAdapter(IPublicationsRepository publicationsRepository) : IDomainPersistencePort
{
    public Task<int> AmountOfPublicationsMadeTodayBy(IUser author) =>
        publicationsRepository.CountPublicationsByUserBetween(author,
                                                              DateTime.UtcNow.Date,
                                                              DateTime.UtcNow.Date.AddDays(1).AddTicks(-1));

    public Task<IPost> PublishNewPost(IUnpublishedPost unpublishedPost) =>
        publicationsRepository.PublishNewPost(unpublishedPost);

    public Task<IRepost> PublishNewRepost(IUnpublishedRepost unpublishedRepost) =>
        publicationsRepository.PublishNewRepost(unpublishedRepost);
}
