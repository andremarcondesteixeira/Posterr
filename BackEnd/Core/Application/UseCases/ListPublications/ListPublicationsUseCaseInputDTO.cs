using Posterr.Core.Shared.ConfigurationInterfaces;

namespace Posterr.Core.Application.UseCases.ListPublicationsWithPagination;

public sealed record ListPublicationsUseCaseInputDTO
{
    public short PageSize { get; }
    public long LastSeenPublicationId { get; }
    public bool IsFirstPage { get; }

    public ListPublicationsUseCaseInputDTO(bool isFirstPage, long lastSeenPublicationId, IDomainConfig domainConfig)
    {
        IsFirstPage = isFirstPage;
        LastSeenPublicationId = lastSeenPublicationId;
        PageSize = isFirstPage ? domainConfig.Pagination.FirstPageSize : domainConfig.Pagination.NextPagesSize;
    }
}
