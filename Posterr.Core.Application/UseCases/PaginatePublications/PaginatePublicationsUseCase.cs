using Posterr.Core.Application.Interfaces;
using Posterr.Core.Domain.Boundaries.Persistence;

namespace Posterr.Core.Application.UseCases.PaginatePublications;

public sealed class PaginatePublicationsUseCase(IPublicationsRepository repository)
    : IUseCase<PaginatePublicationRequest, IList<IPublication>>
{
    public Task<IList<IPublication>> Run(PaginatePublicationRequest request) =>
        repository.Paginate(request.LastSeenRow, request.PageSize);
}
