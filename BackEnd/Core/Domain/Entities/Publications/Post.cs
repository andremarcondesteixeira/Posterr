using Posterr.Core.Boundaries.Configuration;
using Posterr.Core.Boundaries.EntitiesInterfaces;

namespace Posterr.Core.Domain.Entities.Publications;

// Looking from the domain model's perspective, a Post could be uniquely identified by the combination of
// the author identity plus the publication date and maybe the content itself.
// But this is not good for communication between the stakeholders.
// Using a simple numeric Id makes talking about a specific post much easier even in the non technical departments.
// Example of a dialog using a composite identity for Posts:
//         Jeff: Hey Jimmy, we need to delete the post from UserA from March 13th 2024 at 12:05 P.M because it violated our terms of conduct
//         Jimmy: Can't find it. Did you say P.M or A.M?
// Now compare it against a dialog using a simple numeric identity for posts:
//         Jeff: Hey Jimmy, we need to delete the post #1234 because it violated our terms of conduct
//         Jimmy: Done!
// Which one is easier? ;)
public sealed record Post : IPost
{
    private readonly PostContent _content;
    public long Id { get; }
    public IUser Author { get; }
    public DateTime PublicationDate { get; }
    public string Content { get => _content.Value; }

    private Post(
        long id,
        IUser author,
        DateTime publicationDate,
        string content,
        IDomainConfig domainConfig
    )
    {
        ArgumentNullException.ThrowIfNull(author, nameof(author));
        Id = id;
        Author = author;
        PublicationDate = publicationDate;
        _content = new PostContent(content, domainConfig);
    }

    public static PostBuilder Builder() => new();

    // 4 parameters for is too much for me!
    // Builder for the go!
    // Yes, I'm just flexing my Design Patterns muscles a bit ;)
    public sealed record PostBuilder
    {
        long? Id { get; set; }
        IUser? Author { get; set; }
        DateTime? PublicationDate { get; set; }
        string? Content { get; set; }
        IDomainConfig? DomainConfig { get; set; }

        public PostBuilder WithId(long id) => this with { Id = id };
        public PostBuilder WithAuthor(IUser author) => this with { Author = author };
        public PostBuilder WithPublicationDate(DateTime when) => this with { PublicationDate = when };
        public PostBuilder WithContent(string content) => this with { Content = content };
        public PostBuilder WithDomainConfig(IDomainConfig domainConfig) => this with { DomainConfig = domainConfig };

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

            if (DomainConfig is null)
            {
                propertiesWithNullValue.Add(nameof(DomainConfig));
            }

            if (propertiesWithNullValue.Count > 0)
            {
                throw new ArgumentException(
                    $"The following properties were not set when building a new Post instance: {string.Join(", ", propertiesWithNullValue)}"
                );
            }

            return new Post((long)Id!, Author!, (DateTime)PublicationDate!, Content!, DomainConfig!);
        }
    }
}
