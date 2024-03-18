namespace Posterr.Core.Domain.Boundaries.Persistence;

public interface IPost : IPublication
{
    long Id { get; }
}
