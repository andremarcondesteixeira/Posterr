namespace Posterr.Core.Application.UseCases.ListPublicationsWithPagination;

// I decided not to call this class as something like "PublicationDTO".
// That's because I want to be clear about the role of this class: To only be used as entry of a page of publications.
// The names "Post", "Repost", "Publication" and other very similar names should be reserved to the Core layer
public record PublicationsPageEntryDTO
{
    public required bool IsRepost { get; init; }
    public required PostData Post { get; init; }
    public RepostData? Repost { get; init; }

    public record PostData(long Id, string AuthorUsername, DateTime PublicationDate, string Content);
    public record RepostData(string AuthorUsername, DateTime PublicationDate);
}
