namespace Posterr.Core.Shared.EntitiesInterfaces;

public interface IPublication
{
    long Id { get; }
    IUser Author { get; }
    DateTime PublicationDate { get; }
    string Content { get; }
}
