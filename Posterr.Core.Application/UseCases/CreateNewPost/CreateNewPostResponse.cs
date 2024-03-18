namespace Posterr.Core.Application.UseCases.CreateNewPost;

public sealed record CreateNewPostResponse
{
    public required long PostId { get; init; }
    public required string AuthorUsername { get; init; }
    public required DateTime PublicationDate { get; init; }
    public required string PostContent { get; init; }
}
