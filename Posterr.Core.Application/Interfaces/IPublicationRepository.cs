
using Posterr.Core.Domain.PersistenceBoundaryInterfaces;

namespace Posterr.Core.Application.Interfaces;

public interface IPublicationRepository
{
    Task<IPost?> FindPostById(long originalPostId);
    IList<IPublication> Paginate(int lastSeenRow, ushort pageSize);
}
