using Posterr.Core.Boundaries.ConfigurationInterface;
using Posterr.Core.Shared.Exceptions;

namespace Posterr.Core.Application.UseCases.ListPublicationsWithPagination;

public sealed record ListPublicationsUseCaseInputDTO
{
    public int PageNumber { get; }
    public short PageSize { get; }
    public int LastRowNumber { get; }

    public ListPublicationsUseCaseInputDTO(int pageNumber, IDomainConfig domainConfig)
    {
        if (pageNumber < 1)
        {
            throw new InvalidPageNumberException(pageNumber);
        }

        PageNumber = pageNumber;

        (LastRowNumber, PageSize) = pageNumber switch
        {
            1 => (0, domainConfig.Pagination.FirstPageSize),
            _ => (pageNumber * 20 - 25, domainConfig.Pagination.NextPagesSize)
        };
    }
}
