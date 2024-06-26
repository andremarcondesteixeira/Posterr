using Posterr.Core.Shared.EntitiesInterfaces;
using Posterr.Core.Shared.PersistenceInterfaces;

namespace Posterr.Core.Application.UseCases.ListPublications;

public sealed class ListPublicationsUseCase(IPublicationsRepository publicationsRepository)
    : IUseCase<ListPublicationsUseCaseInputDTO, IList<IPublication>>
{
    public IList<IPublication> Run(ListPublicationsUseCaseInputDTO input)
    {
        return publicationsRepository.Paginate(input.IsFirstPage, input.LastSeenPublicationId, input.PageSize, input.SortOrder);
    }
}
