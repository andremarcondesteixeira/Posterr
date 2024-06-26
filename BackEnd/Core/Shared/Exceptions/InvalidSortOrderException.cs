namespace Posterr.Core.Shared.Exceptions;

public class InvalidSortOrderException(int sortOrder)
    : PosterrException(
        $"Invalid sort order: {sortOrder}",
        "Provide a valid sort order"
    )
{
}
