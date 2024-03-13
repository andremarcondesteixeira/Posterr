using Posterr.Core.Domain.Publications.Exceptions;
using Posterr.Core.Domain.Users;

namespace Posterr.Core.Domain.Publications;

public sealed record Post
{
    private readonly PostContent _content;
    public long Id { get; }
    public User Author { get; }
    public DateTime PublicationDate { get; }
    public string Content { get => _content.Value; }

    private Post(
        long id,
        User author,
        DateTime publicationDate,
        string content
    )
    {
        ArgumentNullException.ThrowIfNull(author);
        Id = id;
        Author = author;
        PublicationDate = publicationDate;
        _content = new PostContent(content);
    }

    public static PostBuilder Builder() => new();

    public sealed record PostBuilder
    {
        long? Id { get; set; }
        User? Author { get; set; }
        DateTime? PublicationDate { get; set; }
        string? Content { get; set; }

        public PostBuilder WithId(long id) => this with { Id = id };
        public PostBuilder WithAuthor(User author) => this with { Author = author };
        public PostBuilder WithPublicationDate(DateTime when) => this with { PublicationDate = when };
        public PostBuilder WithContent(string content) => this with { Content = content };

        public Post Build()
        {
            List<string> propertiesWithNullValue = [];

            if (Id is null)
            {
                propertiesWithNullValue.Add(nameof(Id));
            }

            if (Author is null)
            {
                propertiesWithNullValue.Add(nameof(Author));
            }

            if (PublicationDate is null)
            {
                propertiesWithNullValue.Add(nameof(PublicationDate));
            }

            if (Content is null)
            {
                propertiesWithNullValue.Add(nameof(Content));
            }

            if (propertiesWithNullValue.Count > 0)
            {
                throw new PostBuilderStateHadNullValuesOnBuildException(propertiesWithNullValue);
            }

            return new Post((long)Id!, Author!, (DateTime)PublicationDate!, Content!);
        }
    }
}
