using FakeItEasy;
using Posterr.Core.Boundaries.ConfigurationInterface;

namespace Posterr.Core.Domain.EntitiesTests;

public static class Fake
{
    public static IDomainConfig DomainConfig()
    {
        var domainConfig = A.Fake<IDomainConfig>(x => x.Strict(StrictFakeOptions.AllowToString));
        var pagination = A.Fake<IDomainConfig.IPaginationConfig>(x => x.Strict(StrictFakeOptions.AllowToString));

        A.CallTo(() => domainConfig.MaxPostLength).Returns((uint)777);
        A.CallTo(() => domainConfig.MaxAllowedDailyPublicationsByUser).Returns((ushort)5);
        A.CallTo(() => domainConfig.Pagination).Returns(pagination);
        A.CallTo(() => pagination.FirstPageSize).Returns((short)15);
        A.CallTo(() => pagination.NextPagesSize).Returns((short)20);

        return domainConfig;
    }
}
