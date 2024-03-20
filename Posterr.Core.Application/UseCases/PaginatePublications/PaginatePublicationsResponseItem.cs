using Newtonsoft.Json;

namespace Posterr.Core.Application.UseCases.PaginatePublications;

public record PaginatePublicationsResponseItem
{
    public required bool IsRepost { get; init; }

    public required PostData Post { get; init; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public RepostData? Repost { get; init; }

    public record PostData(long PostId, string AuthorUsername, DateTime PublicationDate, string Content);

    public record RepostData(string AuthorUsername, DateTime PublicationDate);
}

