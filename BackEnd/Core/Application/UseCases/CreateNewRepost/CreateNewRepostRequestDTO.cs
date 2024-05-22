namespace Posterr.Core.Application.UseCases.CreateNewRepost;

public sealed record CreateNewRepostRequestDTO
{
    public string AuthorUsername { get; }
    public long OriginalPostId { get; }

    public CreateNewRepostRequestDTO(string authorUsername, long originalPostId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(authorUsername, nameof(authorUsername));
        AuthorUsername = authorUsername;
        OriginalPostId = originalPostId;
    }
}
