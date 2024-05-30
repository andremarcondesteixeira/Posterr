using Posterr.Core.Boundaries.EntitiesInterfaces;

namespace Posterr.Core.Shared.Exceptions;

public class DuplicatedRepostException(IUnpublishedRepost unpublishedRepost)
    : PosterrException(
        $"The post with ID {unpublishedRepost.OriginalPost.Id} was already reposted by the user {unpublishedRepost.Author.Username}",
        "Search for the repost or repost a different post"
    )
{
}
