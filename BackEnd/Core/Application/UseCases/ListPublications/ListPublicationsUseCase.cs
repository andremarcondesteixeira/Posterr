using Posterr.Core.Boundaries.Persistence;
using Posterr.Core.Boundaries.EntitiesInterfaces;

namespace Posterr.Core.Application.UseCases.ListPublicationsWithPagination;

public sealed class ListPublicationsUseCase(IPublicationsRepository publicationsRepository)
    : IUseCase<ListPublicationsUseCaseInputDTO, IList<IPublication>>
{
    public IList<IPublication> Run(ListPublicationsUseCaseInputDTO input) =>
        publicationsRepository.Paginate(input.LastRowNumber, input.PageSize);
}
