using Posterr.Core.Boundaries.Persistence;
using Posterr.Core.Boundaries.EntitiesInterfaces;

namespace Posterr.Core.Application.UseCases.ListPublicationsWithPagination;

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
