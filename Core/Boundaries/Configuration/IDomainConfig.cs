namespace Posterr.Core.Boundaries.Configuration;

public interface IDomainConfig
{
    uint MaxPostLength { get; }
    ushort MaxAllowedDailyPublicationsByUser { get; }
}
