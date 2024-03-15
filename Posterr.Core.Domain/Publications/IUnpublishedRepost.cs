using Posterr.Core.Domain.Users;

namespace Posterr.Core.Domain.Publications;

public interface IUnpublishedRepost
{
    IPost OriginalPost { get; }
    IUser Author { get; }
}
