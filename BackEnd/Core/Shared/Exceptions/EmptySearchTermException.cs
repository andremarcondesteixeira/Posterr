namespace Posterr.Core.Shared.Exceptions;

public sealed class EmptySearchTermException()
    : PosterrException(
        "The search term must not be empty",
        "Provide a non-empty search term"
    )
{
}