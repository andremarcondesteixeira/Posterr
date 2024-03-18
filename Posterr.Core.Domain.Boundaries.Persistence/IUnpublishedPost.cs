namespace Posterr.Core.Domain.Boundaries.Persistence;

public interface IUnpublishedPost
{
    string Content { get; }
    IUser Author { get; }
}
