namespace Posterr.Core.Domain.Boundaries.Persistence;

public interface IRepost : IPublication
{
    IPost OriginalPost { get; }
}
