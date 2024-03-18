namespace Posterr.Core.Domain.Boundaries.Configuration;

public interface IDomainConfig
{
    uint MaxPostLength { get; }
    ushort MaxAllowedDailyPublicationsByUser { get; }
}
