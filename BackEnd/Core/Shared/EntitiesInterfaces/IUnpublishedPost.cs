namespace Posterr.Core.Shared.EntitiesInterfaces;

public interface IUnpublishedPost
{
    string Content { get; }
    IUser Author { get; }
}
