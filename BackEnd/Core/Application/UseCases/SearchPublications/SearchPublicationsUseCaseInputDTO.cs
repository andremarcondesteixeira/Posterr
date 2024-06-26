using Posterr.Core.Shared.ConfigurationInterfaces;
using Posterr.Core.Shared.Exceptions;

namespace Posterr.Core.Application.UseCases.SearchPublications;

public record SearchPublicationsUseCaseInputDTO
{
    public string SearchTerm { get; }

    public SearchPublicationsUseCaseInputDTO(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            throw new EmptySearchTermException();
        }

        SearchTerm = searchTerm;
    }
}
