namespace Posterr.Core.Shared.Exceptions;

public class PublicationNotFoundException(long publicationId)
    : PosterrException(
        $"No publication found with Id {publicationId}",
        "Check that you provided the correct search parameters and that the publication is persisted in the database"
    )
{
}
