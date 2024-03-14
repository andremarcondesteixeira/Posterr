using Posterr.Core.Domain.Users;

namespace Posterr.Core.Domain.Publications;

public interface IPost
{
    long Id { get; }
    IUser Author { get; }
    DateTime PublicationDate { get; }
    string Content { get; }
}
