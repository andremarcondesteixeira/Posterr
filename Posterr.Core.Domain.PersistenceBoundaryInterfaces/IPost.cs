namespace Posterr.Core.Domain.PersistenceBoundaryInterfaces;

public interface IPost : IPublication
{
    long Id { get; }
}
