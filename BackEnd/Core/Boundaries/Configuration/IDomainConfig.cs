namespace Posterr.Core.Boundaries.Configuration;

public interface IDomainConfig
{
    uint MaxPostLength { get; }
    ushort MaxAllowedDailyPublicationsByUser { get; }
    IPaginationConfig Pagination { get; }

    public interface IPaginationConfig
    {
        short FirstPageSize { get; }
        short NextPagesSize { get; }
    }
}
