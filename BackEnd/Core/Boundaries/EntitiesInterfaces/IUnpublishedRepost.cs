namespace Posterr.Core.Boundaries.EntitiesInterfaces;

public interface IUnpublishedRepost
{
    IUser Author { get; }
    IPost OriginalPost { get; }
}
