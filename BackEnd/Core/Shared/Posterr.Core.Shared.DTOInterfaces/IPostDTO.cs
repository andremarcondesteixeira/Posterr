namespace Posterr.Core.Shared.DTOInterfaces;

public interface IPostDTO
{
    long Id { get; }
    bool IsRepost { get; }
    string AuthorUsername { get; }
    DateTime PublicationDate { get; }
    string Content { get; }
}
