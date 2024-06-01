namespace Posterr.Core.Shared.Exceptions;

public class CannotRepostRepostException(string authorUsername, long originalRepostId)
    : PosterrException(
        $"User {authorUsername} tried to repost the repost with ID {originalRepostId}",
        "Check if you selected the correct post or repost a normal post instead"
    )
{
}
