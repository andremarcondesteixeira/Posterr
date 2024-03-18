namespace Posterr.Infrastructure.Persistence.DbEntities;

public class BaseDbEntity
{
    public long Id { get; set; }
    public DateTime CreatedAt { get; set; }
}
