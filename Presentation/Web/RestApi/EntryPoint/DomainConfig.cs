using Posterr.Core.Boundaries.Configuration;

namespace Posterr.Presentation.Web.RestApi.EntryPoint;

public sealed record DomainConfig(IConfiguration Configuration) : IDomainConfig
{
    public uint MaxPostLength =>
        Configuration.GetRequiredSection("Domain").GetRequiredSection("MaxPostLength").Get<uint>();

    public ushort MaxAllowedDailyPublicationsByUser =>
        Configuration.GetRequiredSection("Domain").GetRequiredSection("MaxAllowedDailyPublicationsByUser").Get<ushort>();
}
