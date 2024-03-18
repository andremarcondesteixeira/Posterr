namespace Posterr.Core.Domain.Boundaries.Persistence;

public interface IPublication
{
    IUser Author { get; }
    DateTime PublicationDate { get; }
    string Content { get; }
}
