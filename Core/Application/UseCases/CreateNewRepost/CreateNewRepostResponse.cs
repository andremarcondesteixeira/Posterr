namespace Posterr.Core.Application.UseCases.CreateNewRepost;

public sealed record CreateNewRepostResponse
{
    public required string RepostAuthorUsername { get; init; }
    public required DateTime RepostPublicationDate { get; init; }
    public required Original OriginalPost { get; init; }

    public sealed record Original
    {
        public required long Id { get; init; }
        public required string AuthorUsername { get; init; }
        public required DateTime PublicationDate { get; init; }
        public required string Content { get; init; }
    }
}
