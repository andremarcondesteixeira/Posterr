namespace Posterr.Core.Domain.PersistenceBoundaryInterfaces;

public interface IPublication
{
    IUser Author { get; }
    DateTime PublicationDate { get; }
    string Content { get; }
}
