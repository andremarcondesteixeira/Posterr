namespace Posterr.Core.Shared.Exceptions;

public sealed class UserNotFoundException(string username)
    : PosterrException(
        $"No user found with username \"{username}\".",
        "Check if the user is actually persisted in the database and if you typed the username correctly."
    )
{
}
