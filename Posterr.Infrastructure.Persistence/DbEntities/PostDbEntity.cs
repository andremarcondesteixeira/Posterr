using System.ComponentModel.DataAnnotations.Schema;

namespace Posterr.Infrastructure.Persistence.DbEntities;

[Table("Posts")]
public class PostDbEntity : BaseDbEntity
{
    public long UserId { get; set; }
    public required string Content { get; set; }

    [ForeignKey(nameof(UserId))]
    public required UserDbEntity User { get; set; }
}

