using FakeItEasy;
using Posterr.Core.Boundaries.Configuration;
using Posterr.Core.Boundaries.EntitiesInterfaces;
using Posterr.Core.Domain.Entities;
using Posterr.Core.Domain.Entities.Publications;
using Posterr.Core.Shared.Exceptions;

namespace Posterr.Core.Domain.EntitiesTests.Publications;

public class UnpublishedRepostTests
{
    private readonly IDomainConfig _domainConfig;

    public UnpublishedRepostTests()
    {
        _domainConfig = A.Fake<IDomainConfig>();
        A.CallTo(() => _domainConfig.MaxAllowedDailyPublicationsByUser).Returns((ushort) 5);
    }

    [Fact]
    public void GivenValidParameters_WhenInstantiatingUnpublishedRepostEntity_ThenSucceed()
    {
        var author = A.Fake<IUser>();
        var originalPost = A.Fake<IPost>();
        var unpublishedRepost = new UnpublishedRepost(author, originalPost, _domainConfig);

        Assert.Equal(author, unpublishedRepost.Author);
        Assert.Equal(originalPost, unpublishedRepost.OriginalPost);
        Assert.Equal(_domainConfig, unpublishedRepost.DomainConfig);
    }

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
    [Fact]
    public void GivenNullAuthor_WhenInstantiatingUnpublishedRepostEntity_ThenThrowException()
    {
        Assert.Throws<ArgumentNullException>(() => new UnpublishedRepost(null, A.Fake<IPost>(), _domainConfig));
    }

    [Fact]
    public void GivenNullOriginalPost_WhenInstantiatingUnpublishedRepostEntity_ThenThrowException()
    {
        Assert.Throws<ArgumentNullException>(() => new UnpublishedRepost(A.Fake<IUser>(), null, _domainConfig));
    }

    [Fact]
    public void GivenNullDomainConfig_WhenInstantiatingUnpublishedRepostEntity_ThenThrowException()
    {
        Assert.Throws<ArgumentNullException>(() => new UnpublishedRepost(A.Fake<IUser>(), A.Fake<IPost>(), null));
    }
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

    [Fact]
    public async Task GivenUserHasNotReachedMaxAllowedDailyPublications_WhenPublishingUnpublishedPost_ThenSucceed()
    {
        var author = A.Fake<IUser>();
        var originalPost = A.Fake<IPost>();
        var unpublishedRepost = new UnpublishedRepost(author, originalPost, _domainConfig);
        var now = DateTime.UtcNow;

        var domainPersistencePort = A.Fake<IDomainPersistencePort>();
        A.CallTo(() => domainPersistencePort.AmountOfPublicationsMadeTodayBy(author)).Returns(Task.FromResult(0));
        A.CallTo(() => domainPersistencePort.PublishNewRepost(unpublishedRepost)).Returns(new Repost(author, originalPost, now));

        var repost = await unpublishedRepost.Publish(domainPersistencePort);

        Assert.Equal(author, repost.Author);
        Assert.Equal(originalPost, repost.OriginalPost);
        Assert.Equal(now, repost.PublicationDate);
    }

    [Fact]
    public async Task GivenUserHasReachedMaxAllowedDailyPublications_WhenPublishingUnpublishedPost_ThenThrowException()
    {
        var author = A.Fake<IUser>();
        var unpublishedRepost = new UnpublishedRepost(author, A.Fake<IPost>(), _domainConfig);

        var domainPersistencePort = A.Fake<IDomainPersistencePort>();
        A.CallTo(() => domainPersistencePort.AmountOfPublicationsMadeTodayBy(author))
            .Returns(Task.FromResult((int) _domainConfig.MaxAllowedDailyPublicationsByUser));

        await Assert.ThrowsAsync<MaxAllowedDailyPublicationsByUserExceededException>(
            () => unpublishedRepost.Publish(domainPersistencePort)
        );
    }
}
