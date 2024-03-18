using Posterr.Core.Domain.Boundaries.Configuration;
using Posterr.Core.Domain.Boundaries.Persistence;
using Posterr.Core.Domain.Publications.Exceptions;

namespace Posterr.Core.Domain.Publications;

// I didn't used inheritance here because Post has one more field, therefore, Post would need to inherit from UnpublishedPost.
// This could lead to some challenges in communication with the stakeholders: Is a Post a form of an UnpublishedPost?
// Moreover, Post and UnpublishedPost have different responsibilities and could change for different reasons,
// and because of this, I don't see one as being a subtype of the other
public sealed record UnpublishedPost : IUnpublishedPost
{
    private readonly PostContent _content;
    public string Content { get => _content.Value; }
    public IUser Author { get; }
    public IDomainConfig DomainConfig { get; }

    // Here, I decided to get an User entity instead of just the author name.
    // That's because I understand that checking if the User exists is an Application Business Rule.
    // Therefore, the Application layer should pass the Domain layer an already verified user.
    public UnpublishedPost(IUser author, string content, IDomainConfig domainConfig)
    {
        ArgumentNullException.ThrowIfNull(author);
        ArgumentNullException.ThrowIfNull(domainConfig);

        // Again, validation is done in constructors to make illegal states unrepresentable.
        // In this case, PostContent is checking if the rules about the post content are being applied.
        _content = new PostContent(content, domainConfig);
        Author = author;
        DomainConfig = domainConfig;
    }

    public async Task<IPost> Publish(IDomainPersistencePort persistencePort)
    {
        if (await persistencePort.AmountOfPublicationsMadeTodayBy(Author) >= DomainConfig.MaxAllowedDailyPublicationsByUser)
        {
            throw new MaxAllowedDailyPublicationsByUserExceededException(Author, DomainConfig);
        }

        return await persistencePort.PublishNewPost(this);
    }
}
