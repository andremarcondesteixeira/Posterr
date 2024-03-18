namespace Posterr.Core.Domain.Boundaries.Persistence;

public interface IUnpublishedRepost
{
    IUser Author { get; }
    IPost OriginalPost { get; }
}
