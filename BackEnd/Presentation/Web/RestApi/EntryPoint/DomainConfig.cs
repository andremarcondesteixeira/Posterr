using Posterr.Core.Boundaries.Configuration;

namespace Posterr.Presentation.Web.RestApi.EntryPoint;

public sealed record DomainConfig(IConfiguration Configuration) : IDomainConfig
{
    public uint MaxPostLength => Configuration
        .GetRequiredSection("Domain")
        .GetRequiredSection("MaxPostLength")
        .Get<uint>();

    public ushort MaxAllowedDailyPublicationsByUser => Configuration
        .GetRequiredSection("Domain")
        .GetRequiredSection("MaxAllowedDailyPublicationsByUser")
        .Get<ushort>();

    public IDomainConfig.IPaginationConfig Pagination => new PaginationConfig(Configuration);

    public sealed record PaginationConfig(IConfiguration Configuration) : IDomainConfig.IPaginationConfig
    {
        public short FirstPageSize => Configuration
            .GetRequiredSection("Domain")
            .GetRequiredSection("Pagination")
            .GetRequiredSection("FirstPageSize")
            .Get<short>();

        public short NextPagesSize => Configuration
            .GetRequiredSection("Domain")
            .GetRequiredSection("Pagination")
            .GetRequiredSection("NextPagesSize")
            .Get<short>();
    }
}
