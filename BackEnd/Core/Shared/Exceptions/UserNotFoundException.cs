namespace Posterr.Core.Shared.Exceptions;

public sealed class UserNotFoundException : PosterrException
{
    public UserNotFoundException(string username) : base(
        $"No user found with username \"{username}\".",
        "Check if the user is actually persisted in the database and if you typed the username correctly."
    )
    { }

    public UserNotFoundException(long userId) : base(
        $"No user found with id {userId}.",
        "Check if the user is actually persisted in the database and if you typed the user id correctly."
    )
    { }
}
