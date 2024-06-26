using Posterr.Core.Shared.EntitiesInterfaces;
using Posterr.Core.Shared.PersistenceInterfaces;

namespace Posterr.Core.Application.UseCases.SearchPublications;

public class SearchPublicationsUseCase(IPublicationsRepository publicationsRepository) : IUseCase<SearchPublicationsUseCaseInputDTO, IList<IPublication>>
{
    public IList<IPublication> Run(SearchPublicationsUseCaseInputDTO input)
    {
        return publicationsRepository.Search(input.SearchTerm);
    }
}
