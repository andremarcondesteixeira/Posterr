﻿using Posterr.Core.Domain.Exceptions;
using Posterr.Core.Domain.Users;

namespace Posterr.Core.Domain.Publications;

// I didn't used inheritance here because Post has one more field, therefore, Post would need to inherit from UnpublishedPost.
// This could lead to some challenges in communication with the stakeholders: Is a Post a form of an UnpublishedPost?
// Moreover, Post and UnpublishedPost have different responsibilities and could change for different reasons,
// and because of this, I don't see one as being a subtype of the other
public sealed record UnpublishedPost
{
    // This doesn't feel like the appropriate place for this const
    public const ushort MAX_ALLOWED_DAILY_PUBLICATIONS_BY_USER = 5;

    private readonly PostContent _content;
    public string Content { get => _content.Value; }
    public User Author { get; }

    // Here, I decided to get an User entity instead of just the author name.
    // That's because I understand that checking if the User exists is an Application Business Rule.
    // Therefore, the Application layer should pass the Domain layer an already verified user.
    public UnpublishedPost(User author, string content)
    {
        ArgumentNullException.ThrowIfNull(author);

        // Again, validation is done in constructors to enforce illegal states to be unrepresentable.
        // In this case, PostContent is checking if the rules about the post content are being applied.
        _content = new PostContent(content);
        Author = author;
    }

    public async Task<Post> Publish(IDomainPersistencePort persistencePort)
    {
        if (await persistencePort.AmountOfPublicationsMadeTodayBy(Author) >= MAX_ALLOWED_DAILY_PUBLICATIONS_BY_USER)
        {
            throw new MaxAllowedDailyPublicationsByUserExceededException(
                $"The user {Author.Username} is not allowed to make more than {MAX_ALLOWED_DAILY_PUBLICATIONS_BY_USER} publications in a single day."
            );
        }

        return await persistencePort.PublishNewPost(this);
    }
}
