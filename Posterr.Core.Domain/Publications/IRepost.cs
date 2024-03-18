namespace Posterr.Core.Domain.Publications;

public interface IRepost : IPublication
{
    IPost OriginalPost { get; }
}
