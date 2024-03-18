using Posterr.Core.Domain.Boundaries.Configuration;
using Posterr.Core.Domain.Boundaries.Persistence;
using Posterr.Core.Domain.Publications.Exceptions;

namespace Posterr.Core.Domain.Publications;

public sealed record UnpublishedRepost(IUser Author, IPost OriginalPost, IDomainConfig DomainConfig) : IUnpublishedRepost
{
    public IUser Author { get; } = Author ?? throw new ArgumentNullException(nameof(Author));

    public IPost OriginalPost { get; } = OriginalPost ?? throw new ArgumentNullException(nameof(OriginalPost));

    public IDomainConfig DomainConfig { get; } = DomainConfig ?? throw new ArgumentNullException(nameof(DomainConfig));

    public async Task<IRepost> Publish(IDomainPersistencePort persistencePort)
    {
        if (await persistencePort.AmountOfPublicationsMadeTodayBy(Author) >= DomainConfig.MaxAllowedDailyPublicationsByUser)
        {
            throw new MaxAllowedDailyPublicationsByUserExceededException(Author, DomainConfig);
        }

        return await persistencePort.PublishNewRepost(this);
    }
}
