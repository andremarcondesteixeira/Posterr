using Posterr.Core.Domain.Users;

namespace Posterr.Core.Domain.Publications;

public interface IUnpublishedPost
{
    string Content { get; }
    IUser Author { get; }
}
