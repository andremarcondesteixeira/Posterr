namespace Posterr.Core.Shared.ConfigurationInterfaces;

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
