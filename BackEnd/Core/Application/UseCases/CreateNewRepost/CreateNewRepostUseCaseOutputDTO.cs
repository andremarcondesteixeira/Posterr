namespace Posterr.Core.Application.UseCases.CreateNewRepost;

public sealed record CreateNewRepostUseCaseOutputDTO
{
    public required string AuthorUsername { get; init; }
    public required DateTime PublicationDate { get; init; }
    public required OriginalPostData OriginalPost { get; init; }

    public sealed record OriginalPostData
    {
        public required long Id { get; init; }
        public required string AuthorUsername { get; init; }
        public required DateTime PublicationDate { get; init; }
        public required string Content { get; init; }
    }
}
