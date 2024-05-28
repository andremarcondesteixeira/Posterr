namespace Posterr.Core.Application.UseCases.ListPublicationsWithPagination;

public class InvalidPageNumberException(int pageNumber) : Exception($"Page number must be a positive integer bigger than 0. Got {pageNumber} instead.")
{
}
