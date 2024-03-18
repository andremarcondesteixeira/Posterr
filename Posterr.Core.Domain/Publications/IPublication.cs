using Posterr.Core.Domain.Users;

namespace Posterr.Core.Domain.Publications;

public interface IPublication
{
    IUser Author { get; }
    DateTime PublicationDate { get; }
    string Content { get; }
}
