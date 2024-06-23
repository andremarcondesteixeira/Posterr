using Posterr.Core.Shared.EntitiesInterfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace Posterr.Infrastructure.Persistence.DbEntities;

[Table("Publications")]
public class PublicationDbEntity : BaseDbEntity
{
    public required long AuthorId { get; set; }
    public required string AuthorUsername { get; set; }
    public required string Content { get; set; }
    public required DateTime PublicationDate { get; set; }
    public long? OriginalPostId { get; set; }
    public long? OriginalPostAuthorId { get; set; }
    public string? OriginalPostAuthorUsername { get; set; }
    public string? OriginalPostContent { get; set; }
    public DateTime? OriginalPostPublicationDate { get; set; }

    [ForeignKey(nameof(AuthorId))]
    public UserDbEntity? Author { get; set; }

    [ForeignKey(nameof(OriginalPostId))]
    public PublicationDbEntity? OriginalPost { get; set; }

    public IPost ToIPost() => new Post(Id, new User(AuthorId, AuthorUsername), PublicationDate, Content);

    public IRepost ToIRepost() => new Repost(
        Id,
        new User(AuthorId, AuthorUsername),
        PublicationDate,
        Content,
        new Post(
            (long)OriginalPostId!,
            new User((long)OriginalPostAuthorId!, OriginalPostAuthorUsername!),
            (DateTime)OriginalPostPublicationDate!,
            OriginalPostContent!
        )
    );

    public IPublication ToIPublication()
    {
        if (OriginalPostAuthorId == null)
        {
            return ToIPost();
        }

        return ToIRepost();
    }

    public record Post(long Id, IUser Author, DateTime PublicationDate, string Content) : IPost;

    public record Repost(long Id, IUser Author, DateTime PublicationDate, string Content, IPost OriginalPost) : IRepost;

    public record User(long Id, string Username) : IUser;
}
