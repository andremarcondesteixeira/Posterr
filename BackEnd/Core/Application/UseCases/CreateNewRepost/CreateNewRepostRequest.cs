namespace Posterr.Core.Application.UseCases.CreateNewRepost;

public sealed record CreateNewRepostRequest
{
    public string AuthorUsername { get; }
    public long OriginalPostId { get; }

    public CreateNewRepostRequest(string authorUsername, long originalPostId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(authorUsername, nameof(authorUsername));
        AuthorUsername = authorUsername;
        OriginalPostId = originalPostId;
    }
}
