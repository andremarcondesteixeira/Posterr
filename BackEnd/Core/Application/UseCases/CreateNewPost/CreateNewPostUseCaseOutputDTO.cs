namespace Posterr.Core.Application.UseCases.CreateNewPost;

public sealed record CreateNewPostUseCaseOutputDTO
{
    public required long Id { get; init; }
    public required string AuthorUsername { get; init; }
    public required DateTime PublicationDate { get; init; }
    public required string Content { get; init; }
}
