namespace Posterr.Core.Shared.Exceptions;

public class InvalidPageNumberException(int pageNumber)
    : PosterrException(
        $"The page number must be an integer bigger than 0. Got {pageNumber} instead.",
        "Provide an integer bigger than zero."
    )
{
}
