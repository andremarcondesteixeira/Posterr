using FakeItEasy;
using Posterr.Core.Domain.Exceptions;
using Posterr.Core.Domain.Publications;
using Posterr.Core.Domain.Users;

namespace Posterr.Core.Domain.Tests.Publications;

public class UnpublishedRepostTests
{
    [Fact]
    public void GivenValidParameters_WhenInstantiatingUnpublishedRepostEntity_ThenSucceed()
    {
        var author = A.Fake<IUser>();
        var originalPost = A.Fake<IPost>();
        var unpublishedRepost = new UnpublishedRepost(author, originalPost);

        Assert.Equal(author, unpublishedRepost.Author);
        Assert.Equal(originalPost, unpublishedRepost.OriginalPost);
    }

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
    [Fact]
    public void GivenNullAuthor_WhenInstantiatingUnpublishedRepostEntity_ThenThrowException()
    {
        Assert.Throws<ArgumentNullException>(() => new UnpublishedRepost(null, A.Fake<IPost>()));
    }

    [Fact]
    public void GivenNullOriginalPost_WhenInstantiatingUnpublishedRepostEntity_ThenThrowException()
    {
        Assert.Throws<ArgumentNullException>(() => new UnpublishedRepost(A.Fake<IUser>(), null));
    }
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

    [Fact]
    public async Task GivenUserHasNotReachedMaxAllowedDailyPublications_WhenPublishingUnpublishedPost_ThenSucceed()
    {
        var author = A.Fake<IUser>();
        var originalPost = A.Fake<IPost>();
        var unpublishedRepost = new UnpublishedRepost(author, originalPost);
        var now = DateTime.UtcNow;

        var domainPersistencePort = A.Fake<IDomainPersistencePort>();
        A.CallTo(() => domainPersistencePort.AmountOfPublicationsMadeTodayBy(author)).Returns(Task.FromResult<ushort>(0));
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
        var unpublishedRepost = new UnpublishedRepost(author, A.Fake<IPost>());

        var domainPersistencePort = A.Fake<IDomainPersistencePort>();
        A.CallTo(() => domainPersistencePort.AmountOfPublicationsMadeTodayBy(author))
            .Returns(Task.FromResult<ushort>(UnpublishedRepost.MAX_ALLOWED_DAILY_PUBLICATIONS_BY_USER));

        await Assert.ThrowsAsync<MaxAllowedDailyPublicationsByUserExceededException>(
            () => unpublishedRepost.Publish(domainPersistencePort)
        );
    }
}
