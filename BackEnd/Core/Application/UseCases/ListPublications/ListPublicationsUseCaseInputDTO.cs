using Posterr.Core.Shared.ConfigurationInterfaces;
using Posterr.Core.Shared.Enums;
using Posterr.Core.Shared.Exceptions;

namespace Posterr.Core.Application.UseCases.ListPublications;

public sealed record ListPublicationsUseCaseInputDTO
{
    public short PageSize { get; }
    public long LastSeenPublicationId { get; }
    public bool IsFirstPage { get; }
    public SortOrder SortOrder { get; }

    public ListPublicationsUseCaseInputDTO(bool isFirstPage, long lastSeenPublicationId, IDomainConfig domainConfig, int sortOrder)
    {
        if (!Enum.IsDefined(typeof(SortOrder), sortOrder))
        {
            throw new InvalidSortOrderException(sortOrder);
        }

        IsFirstPage = isFirstPage;
        LastSeenPublicationId = lastSeenPublicationId;
        PageSize = isFirstPage ? domainConfig.Pagination.FirstPageSize : domainConfig.Pagination.NextPagesSize;
        SortOrder = (SortOrder)sortOrder;
    }
}
