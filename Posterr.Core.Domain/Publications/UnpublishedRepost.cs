using Posterr.Core.Domain.Exceptions;
using Posterr.Core.Domain.Users;

namespace Posterr.Core.Domain.Publications;

public record UnpublishedRepost(IUser Author, IPost OriginalPost, IDomainConfig DomainConfig) : IUnpublishedRepost
{
    public IUser Author { get; } = Author ?? throw new ArgumentNullException(nameof(Author));

    public IPost OriginalPost { get; } = OriginalPost ?? throw new ArgumentNullException(nameof(OriginalPost));

    public IDomainConfig DomainConfig { get; } = DomainConfig ?? throw new ArgumentNullException(nameof(DomainConfig));

    public async Task<IRepost> Publish(IDomainPersistencePort persistencePort)
    {
        if (await persistencePort.AmountOfPublicationsMadeTodayBy(Author) >= DomainConfig.MaxAllowedDailyPublicationsByUser)
        {
            throw new MaxAllowedDailyPublicationsByUserExceededException(
                $"The user {Author.Username} is not allowed to make more than {DomainConfig.MaxAllowedDailyPublicationsByUser} publications in a single day."
            );
        }

        return await persistencePort.PublishNewRepost(this);
    }
}
