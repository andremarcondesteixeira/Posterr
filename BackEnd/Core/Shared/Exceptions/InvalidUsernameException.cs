namespace Posterr.Core.Shared.Exceptions;

public sealed class InvalidUsernameException(string Value)
    : PosterrException(
        $"Username must not be empty and must contain only alphanumeric characters. Got \"{Value}\", instead.",
        "Provide a username containing at least 1 alphanumeric character, and alphanumeric characters only."
    )
{
}
