namespace Posterr.Core.Domain.PersistenceBoundaryInterfaces;

public interface IUnpublishedPost
{
    string Content { get; }
    IUser Author { get; }
}
