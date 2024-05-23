namespace Posterr.Core.Application.UseCases.PaginatePublications;

public class InvalidPageNumberException(int pageNumber) : Exception($"Invalid page number: {pageNumber}")
{
}
