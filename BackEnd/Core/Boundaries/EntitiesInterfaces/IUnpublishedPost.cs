namespace Posterr.Core.Boundaries.EntitiesInterfaces;

public interface IUnpublishedPost
{
    string Content { get; }
    IUser Author { get; }
}
