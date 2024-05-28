using Posterr.Core.Boundaries.Configuration;

namespace Posterr.Core.Application.UseCases.ListPublicationsWithPagination;

public sealed record PaginationParameters
{
    public int PageNumber { get; }
    public ushort PageSize { get; }
    public int LastRowNumber { get; }

    public PaginationParameters(int pageNumber, IDomainConfig domainConfig)
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
