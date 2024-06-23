namespace Posterr.Core.Shared.EntitiesInterfaces;

public interface IRepost : IPublication
{
    IPost OriginalPost { get; }
}
