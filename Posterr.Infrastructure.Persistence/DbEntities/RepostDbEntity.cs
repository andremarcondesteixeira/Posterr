using Microsoft.EntityFrameworkCore;
using Posterr.Core.Domain.Boundaries.Persistence;
using System.ComponentModel.DataAnnotations.Schema;

namespace Posterr.Infrastructure.Persistence.DbEntities;

// I decided to create a separate Reposts table so that the content is not duplicated in the database

[Table("Reposts")]
[PrimaryKey(nameof(UserId), nameof(PostId))]
public class RepostDbEntity
{
    public required long UserId { get; set; }
    public required long PostId { get; set; }
    public DateTime CreatedAt { get; set; }

    [ForeignKey(nameof(UserId))]
    public required UserDbEntity User { get; set; }

    [ForeignKey(nameof(PostId))]
    public required PostDbEntity Post { get; set; }

    public IRepost ToIRepost() => new Repost(User.ToIUser(), CreatedAt, Post.Content, Post.ToIPost());

    public record Repost(IUser Author, DateTime PublicationDate, string Content, IPost OriginalPost) : IRepost;
}