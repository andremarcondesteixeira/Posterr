using Posterr.Core.Application.Interfaces;
using Posterr.Core.Domain.PersistenceBoundaryInterfaces;

namespace Posterr.Core.Application.PaginatePublications;

public sealed class PaginatePublicationsUseCase(IPublicationRepository repository)
{
    public IList<IPublication> Run(int lastSeenRow, ushort pageSize) => repository.Paginate(lastSeenRow, pageSize);
}
