
using Posterr.Core.Domain.PersistenceBoundaryInterfaces;

namespace Posterr.Core.Application.Interfaces;

public interface IPublicationsRepository
{
    Task<IPost?> FindPostById(long originalPostId);
    Task<IList<IPublication>> Paginate(int lastSeenRow, ushort pageSize);
}
