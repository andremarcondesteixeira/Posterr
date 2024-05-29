#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.

using FakeItEasy;
using Posterr.Core.Boundaries.Configuration;
using Posterr.Core.Boundaries.EntitiesInterfaces;
using Posterr.Core.Domain.Entities;
using Posterr.Core.Domain.Entities.Publications;
using Posterr.Core.Domain.Entities.Publications.Exceptions;

namespace Posterr.Core.Domain.EntitiesTests.Publications;

public class UnpublishedPostTests
{
    private readonly IDomainConfig _domainConfig;

    public UnpublishedPostTests()
    {
        _domainConfig = A.Fake<IDomainConfig>();
        A.CallTo(() => _domainConfig.MaxAllowedDailyPublicationsByUser).Returns((ushort) 5);
        A.CallTo(() => _domainConfig.MaxPostLength).Returns((uint) 7);
    }

    [Fact]
    public void GivenValidParameters_WhenInstantiatingUnpublishedPostEntity_ThenSucceed()
    {
        var user = A.Fake<IUser>();
        var content = "content";
        var unpublishedPost = new UnpublishedPost(user, content, _domainConfig);
        Assert.Equal(user, unpublishedPost.Author);
        Assert.Equal(content, unpublishedPost.Content);
        Assert.Equal(_domainConfig, unpublishedPost.DomainConfig);
    }

    [Fact]
    public void GivenNullAuthor_WhenInstantiatingUnpublishedPostEntity_ThenThrowException()
    {
        Assert.Throws<ArgumentNullException>(() => new UnpublishedPost(null, "content", _domainConfig));
    }

    [Fact]
    public void GivenNullDomainConfig_WhenInstantiatingUnpublishedPostEntity_ThenThrowException()
    {
        Assert.Throws<ArgumentNullException>(() => new UnpublishedPost(A.Fake<IUser>(), "content", null));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void GivenEmptyPostContent_WhenInstantiatingUnpublishedPostEntity_ThenThrowException(string? content)
    {
        Assert.Throws<EmptyPostContentException>(() => new UnpublishedPost(A.Fake<IUser>(), content, _domainConfig));
    }

    [Fact]
    public async Task GivenUserHasNotReachedMaxAllowedDailyPublications_WhenPublishingUnpublishedPost_ThenSucceed()
    {
        var user = A.Fake<IUser>();
        var content = "content";
        var unpublishedPost = new UnpublishedPost(user, content, _domainConfig);
        var now = DateTime.UtcNow;

        // No, this test it not useless and is not testing only mockery.
        // Actually, it acts as a safeguard that can detect whenever the Publish method changes behavior.
        var persistencePort = A.Fake<IDomainPersistencePort>();
        A.CallTo(() => persistencePort.AmountOfPublicationsMadeTodayBy(user)).Returns(Task.FromResult(0));
        A.CallTo(() => persistencePort.PublishNewPost(unpublishedPost)).Returns(Post
            .Builder()
            .WithId(1)
            .WithAuthor(user)
            .WithPublicationDate(now)
            .WithContent(content)
            .WithDomainConfig(_domainConfig)
            .Build()
        );

        var publishedPost = await unpublishedPost.Publish(persistencePort);

        Assert.Equal(1, publishedPost.Id);
        Assert.Equal(user, publishedPost.Author);
        Assert.Equal(now, publishedPost.PublicationDate);
        Assert.Equal(content, publishedPost.Content);
    }

    [Fact]
    public async Task GivenUserHasReachedMaxAllowedDailyPublications_WhenPublishingUnpublishedPost_ThenThrowException()
    {
        var user = A.Fake<IUser>();
        var unpublishedPost = new UnpublishedPost(user, "content", _domainConfig);

        var persistencePort = A.Fake<IDomainPersistencePort>();
        A.CallTo(() => persistencePort.AmountOfPublicationsMadeTodayBy(user))
            .Returns(_domainConfig.MaxAllowedDailyPublicationsByUser);

        await Assert.ThrowsAsync<MaxAllowedDailyPublicationsByUserExceededException>(
            () => unpublishedPost.Publish(persistencePort)
        );
    }
}
