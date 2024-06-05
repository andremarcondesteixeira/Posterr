using Posterr.Core.Boundaries.ConfigurationInterface;
using Posterr.Core.Boundaries.EntitiesInterfaces;

namespace Posterr.Core.Domain.Entities.Publications;

public sealed record Post(
    long Id,
    IUser Author,
    DateTime PublicationDate,
    string Content,
    IDomainConfig DomainConfig) : IPost
{
    private readonly PostContent _content = new(Content, DomainConfig);

    public long Id { get; } = Id;
    public IUser Author { get; } = Author ?? throw new ArgumentNullException(nameof(Author));
    public DateTime PublicationDate { get; } = PublicationDate;
    public string Content { get => _content.Value; }

    public static PostBuilder Builder() => new();

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
                throw new ArgumentNullException(
                    $"The following properties were not set when building a new Post instance: {string.Join(", ", propertiesWithNullValue)}"
                );
            }

            return new Post((long)Id!, Author!, (DateTime)PublicationDate!, Content!, DomainConfig!);
        }
    }
}
