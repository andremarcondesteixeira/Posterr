using Posterr.Core.Domain.Exceptions;
using Posterr.Core.Domain.Users;

namespace Posterr.Core.Domain.Publications;

public record UnpublishedRepost(IUser Author, IPost OriginalPost) : IUnpublishedRepost
{
    // I don't like this duplication. Need to find a way to remove it.
    public const int MAX_ALLOWED_DAILY_PUBLICATIONS_BY_USER = 5;

    public IUser Author { get; } = Author ?? throw new ArgumentNullException(nameof(UnpublishedRepost.Author));

    public IPost OriginalPost { get; } = OriginalPost ?? throw new ArgumentNullException(nameof(UnpublishedRepost.OriginalPost));

    public async Task<IRepost> Publish(IDomainPersistencePort persistencePort)
    {
        if (await persistencePort.AmountOfPublicationsMadeTodayBy(Author) >= MAX_ALLOWED_DAILY_PUBLICATIONS_BY_USER)
        {
            throw new MaxAllowedDailyPublicationsByUserExceededException(
                $"The user {Author.Username} is not allowed to make more than {MAX_ALLOWED_DAILY_PUBLICATIONS_BY_USER} publications in a single day."
            );
        }

        return await persistencePort.PublishNewRepost(this);
    }
}
