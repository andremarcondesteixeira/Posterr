namespace Posterr.Core.Domain.PersistenceBoundaryInterfaces;

public interface IRepost : IPublication
{
    IPost OriginalPost { get; }
}
