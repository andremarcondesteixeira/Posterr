using Posterr.Core.Shared.ConfigurationInterfaces;
using Posterr.Core.Shared.EntitiesInterfaces;

namespace Posterr.Core.Domain.Entities.Publications;

public record Repost(long Id, IUser Author, DateTime PublicationDate, string Content, IPost OriginalPost, IDomainConfig DomainConfig) : IRepost
{
    private readonly PostContent _content = new(Content, DomainConfig);

    public long Id { get; } = Id;

    public IUser Author { get; } = Author ?? throw new ArgumentNullException(nameof(Author));

    public DateTime PublicationDate { get; } = PublicationDate;

    public string Content { get => _content.Value; }

    public IPost OriginalPost { get; } = OriginalPost ?? throw new ArgumentNullException(nameof(OriginalPost));
}
