namespace Posterr.Core.Boundaries.EntitiesInterfaces;

public interface IRepost : IPublication
{
    IPost OriginalPost { get; }
}
