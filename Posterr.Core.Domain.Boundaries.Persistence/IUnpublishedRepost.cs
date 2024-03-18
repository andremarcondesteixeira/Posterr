namespace Posterr.Core.Domain.PersistenceBoundaryInterfaces;

public interface IUnpublishedRepost
{
    IUser Author { get; }
    IPost OriginalPost { get; }
}
