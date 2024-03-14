using Posterr.Core.Domain.Users;

namespace Posterr.Core.Domain.Publications;

public interface IRepost
{
    IUser Author { get; }

    IPost OriginalPost { get; }

    DateTime PublicationDate { get; }
}
