using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Posterr.Infrastructure.Persistence.DbEntities;

// I decided to create a separate Reposts table so that the content is not duplicated in the database

[Table("Reposts")]
[PrimaryKey(nameof(UserId), nameof(PostId))]
public class RepostDbEntity
{
    public long UserId { get; set; }
    public long PostId { get; set; }
    public DateTime CreatedAt { get; set; }

    [ForeignKey(nameof(UserId))]
    public required UserDbEntity User { get; set; }

    [ForeignKey(nameof(PostId))]
    public required PostDbEntity Post { get; set; }
}