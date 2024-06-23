using Posterr.Core.Shared.EntitiesInterfaces;
using Posterr.Core.Shared.PersistenceInterfaces;

namespace Posterr.Core.Application.UseCases.ListPublications;

public sealed class ListPublicationsUseCase(IPublicationsRepository publicationsRepository)
    : IUseCase<ListPublicationsUseCaseInputDTO, IList<IPublication>>
{
    public IList<IPublication> Run(ListPublicationsUseCaseInputDTO input)
    {
        if (input.IsFirstPage)
        {
            return publicationsRepository.GetNMostRecentPublications(input.PageSize);
        }

        return publicationsRepository.Paginate(input.LastSeenPublicationId, input.PageSize);
    }
}
