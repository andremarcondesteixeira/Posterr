using Posterr.Core.Domain.Users;

namespace Posterr.Core.Domain.Publications;

public interface IUnpublishedRepost
{
    IUser Author { get; }
    IPost OriginalPost { get; }
}
