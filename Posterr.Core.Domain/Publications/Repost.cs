using Posterr.Core.Domain.Users;

namespace Posterr.Core.Domain.Publications;

public record Repost(User Author, Post OriginalPost, DateTime PublicationDate)
{
    public User Author { get; } = Author ?? throw new ArgumentNullException(nameof(Author));

    public Post OriginalPost { get; } = OriginalPost ?? throw new ArgumentNullException(nameof(OriginalPost));
}
