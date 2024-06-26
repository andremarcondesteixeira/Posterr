using Posterr.Core.Shared.ConfigurationInterfaces;
using Posterr.Core.Shared.Exceptions;

namespace Posterr.Core.Application.UseCases.SearchPublications;

public record SearchPublicationsUseCaseInputDTO
{
    public string SearchTerm { get; }
    public short PageSize { get; }
    public long LastSeenPublicationId { get; }
    public bool IsFirstPage { get; }

    public SearchPublicationsUseCaseInputDTO(string searchTerm, long lastSeenPublicationId, bool isFirstPage, IDomainConfig domainConfig)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            throw new EmptySearchTermException();
        }

        SearchTerm = searchTerm;
        IsFirstPage = isFirstPage;
        LastSeenPublicationId = lastSeenPublicationId;
        PageSize = isFirstPage ? domainConfig.Pagination.FirstPageSize : domainConfig.Pagination.NextPagesSize;
    }
}
