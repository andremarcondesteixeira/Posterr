using Posterr.Core.Domain.Boundaries.Persistence;
using System.ComponentModel.DataAnnotations.Schema;

namespace Posterr.Infrastructure.Persistence.DbEntities;

[Table("Posts")]
public class PostDbEntity : BaseDbEntity
{
    public long UserId { get; set; }
    public required string Content { get; set; }

    [ForeignKey(nameof(UserId))]
    public required UserDbEntity User { get; set; }

    public IPost ToIPost() => new Post(Id, User.ToIUser(), CreatedAt, Content);
    
    public record Post(long Id, IUser Author, DateTime PublicationDate, string Content) : IPost;
}

