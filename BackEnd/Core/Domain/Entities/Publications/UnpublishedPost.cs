using Posterr.Core.Boundaries.Configuration;
using Posterr.Core.Boundaries.EntitiesInterfaces;
using Posterr.Core.Boundaries.Persistence;
using Posterr.Core.Shared.Exceptions;

namespace Posterr.Core.Domain.Entities.Publications;

// Post and UnpublishedPost have different responsibilities and could change for different reasons,
// and because of this, I don't see one as being a subtype of the other
public sealed record UnpublishedPost : IUnpublishedPost
{
    private readonly PostContent _content;
    public string Content { get => _content.Value; }
    public IUser Author { get; }   
    public IDomainConfig DomainConfig { get; }

    public UnpublishedPost(IUser author, string content, IDomainConfig domainConfig)
    {
        ArgumentNullException.ThrowIfNull(author);
        ArgumentNullException.ThrowIfNull(domainConfig);

        Author = author;
        _content = new PostContent(content, domainConfig);
        DomainConfig = domainConfig;
    }

    public IPost Publish(IPublicationsRepository publicationsRepository)
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

        return publicationsRepository.PublishNewPost(this);
    }
}
