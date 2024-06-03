using Posterr.Core.Boundaries.Configuration;
using Posterr.Core.Boundaries.EntitiesInterfaces;
using Posterr.Core.Boundaries.Persistence;
using Posterr.Core.Shared.Exceptions;

namespace Posterr.Core.Domain.Entities.Publications;

public sealed record UnpublishedRepost(IUser Author, IPost OriginalPost, IDomainConfig DomainConfig) : IUnpublishedRepost
{
    public IUser Author { get; } = Author ?? throw new ArgumentNullException(nameof(Author));
    public IPost OriginalPost { get; } = OriginalPost ?? throw new ArgumentNullException(nameof(OriginalPost));
    public IDomainConfig DomainConfig { get; } = DomainConfig ?? throw new ArgumentNullException(nameof(DomainConfig));

    public IRepost Publish(IPublicationsRepository publicationsRepository)
    {
        var now = DateTime.UtcNow;
        int publicationsMadeToday = publicationsRepository.CountPublicationsMadeByUserBetweenDateTimeRange(
            Author,
            new DateTime(now.Year, now.Month, now.Day, 0, 0, 0, DateTimeKind.Utc),
            new DateTime(now.Year, now.Month, now.Day, 23, 59, 59, DateTimeKind.Utc)
        );

        if (publicationsMadeToday >= DomainConfig.MaxAllowedDailyPublicationsByUser)
        {
            throw new MaxAllowedDailyPublicationsByUserExceededException(Author, DomainConfig);
        }

        return publicationsRepository.PublishNewRepost(this);
    }
}
