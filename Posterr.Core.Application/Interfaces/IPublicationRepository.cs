
using Posterr.Core.Domain.Publications;

namespace Posterr.Core.Application.Interfaces;

public interface IPublicationRepository
{
    Task<IPost?> FindPostById(long originalPostId);
}
