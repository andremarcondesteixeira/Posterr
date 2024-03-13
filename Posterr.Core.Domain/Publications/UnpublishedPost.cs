using Posterr.Core.Domain.Users;

namespace Posterr.Core.Domain.Publications;

public sealed record UnpublishedPost
{
    private readonly PostContent _content;
    public string Content { get => _content.Value; }
    public User Author { get; }

    public UnpublishedPost(User author, string content)
    {
        ArgumentNullException.ThrowIfNull(author);
        _content = new PostContent(content);
        Author = author;
    }
}
