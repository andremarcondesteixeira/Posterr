namespace Posterr.Core.Shared.Exceptions;

public sealed class PostNotFoundException(long postId)
    : PosterrException(
        $"No post found with ID {postId}",
        "Check if the post is actually persisted in the database and if you typed the ID correctly."
    )
{
}
