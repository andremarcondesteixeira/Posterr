using Posterr.Core.Boundaries.EntitiesInterfaces;

namespace Posterr.Core.Domain.Entities.Publications;

public record Repost(IUser Author, IPost OriginalPost, DateTime PublicationDate) : IRepost
{
    public IUser Author { get; } = Author ?? throw new ArgumentNullException(nameof(Author));

    public DateTime PublicationDate { get; } = PublicationDate;

    public IPost OriginalPost { get; } = OriginalPost ?? throw new ArgumentNullException(nameof(OriginalPost));

    public string Content { get => OriginalPost.Content; }
}
