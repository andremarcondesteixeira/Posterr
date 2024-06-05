#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.

using FakeItEasy;
using Posterr.Core.Boundaries.ConfigurationInterface;
using Posterr.Core.Boundaries.EntitiesInterfaces;
using Posterr.Core.Boundaries.Persistence;
using Posterr.Core.Domain.Entities.Publications;
using Posterr.Core.Shared.Exceptions;

namespace Posterr.Core.Domain.EntitiesTests.Publications;

public class UnpublishedRepostTests
{
    private readonly IDomainConfig _domainConfig = Fake.DomainConfig();

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

    [Fact]
    public void GivenUserHasNotReachedMaxAllowedDailyPublications_WhenPublishingUnpublishedPost_ThenSucceed()
    {
        var author = A.Fake<IUser>();
        var originalPost = A.Fake<IPost>();
        var unpublishedRepost = new UnpublishedRepost(author, originalPost, _domainConfig);
        var now = DateTime.UtcNow;

        var publicationsRepository = A.Fake<IPublicationsRepository>();
        A.CallTo(() => publicationsRepository.CountPublicationsMadeByUserBetweenDateTimeRange(
            author,
            A<DateTime>.Ignored,
            A<DateTime>.Ignored
        )).Returns(0);
        A.CallTo(() => publicationsRepository.PublishNewRepost(unpublishedRepost)).Returns(
            new Repost(1, author, now, "post content", originalPost, _domainConfig)
        );

        var repost = unpublishedRepost.Publish(publicationsRepository);

        Assert.Equal(1, repost.Id);
        Assert.Equal(author, repost.Author);
        Assert.Equal(now, repost.PublicationDate);
        Assert.Equal("post content", repost.Content);
        Assert.Equal(originalPost, repost.OriginalPost);
    }

    [Fact]
    public void GivenUserHasReachedMaxAllowedDailyPublications_WhenPublishingUnpublishedPost_ThenThrowException()
    {
        var author = A.Fake<IUser>();
        var unpublishedRepost = new UnpublishedRepost(author, A.Fake<IPost>(), _domainConfig);

        var publicationsRepository = A.Fake<IPublicationsRepository>();
        A.CallTo(() => publicationsRepository.CountPublicationsMadeByUserBetweenDateTimeRange(
            author,
            A<DateTime>.Ignored,
            A<DateTime>.Ignored
        )).Returns(_domainConfig.MaxAllowedDailyPublicationsByUser);

        Assert.Throws<MaxAllowedDailyPublicationsByUserExceededException>(
            () => unpublishedRepost.Publish(publicationsRepository)
        );
    }
}
