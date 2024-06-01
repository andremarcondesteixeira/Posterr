namespace Posterr.Core.Boundaries.EntitiesInterfaces;

public interface IPublication
{
    long Id { get; }
    IUser Author { get; }
    DateTime PublicationDate { get; }
    string Content { get; }
}
