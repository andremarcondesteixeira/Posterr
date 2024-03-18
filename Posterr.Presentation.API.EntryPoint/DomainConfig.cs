using Posterr.Core.Domain.Boundaries.Configuration;

namespace Posterr.Presentation.API.EntryPoint;

public sealed record DomainConfig(IConfiguration configuration) : IDomainConfig
{
    public uint MaxPostLength =>
        configuration.GetRequiredSection("Domain").GetRequiredSection("MaxPostLength").Get<uint>();

    public ushort MaxAllowedDailyPublicationsByUser =>
        configuration.GetRequiredSection("Domain").GetRequiredSection("MaxAllowedDailyPublicationsByUser").Get<ushort>();
}
