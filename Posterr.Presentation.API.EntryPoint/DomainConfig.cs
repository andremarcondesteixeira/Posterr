using Posterr.Core.Domain.Boundaries.Configuration;

namespace Posterr.Presentation.API.EntryPoint;

public sealed record DomainConfig(IConfiguration Configuration) : IDomainConfig
{
    public uint MaxPostLength =>
        Configuration.GetRequiredSection("Domain").GetRequiredSection("MaxPostLength").Get<uint>();

    public ushort MaxAllowedDailyPublicationsByUser =>
        Configuration.GetRequiredSection("Domain").GetRequiredSection("MaxAllowedDailyPublicationsByUser").Get<ushort>();
}
