using FakeItEasy;
using Posterr.Core.Domain.Exceptions;
using Posterr.Core.Domain.Publications;
using Posterr.Core.Domain.Publications.Exceptions;
using Posterr.Core.Domain.Users;

namespace Posterr.Core.Domain.Tests.Publications;

public class UnpublishedPostTests
{
    [Fact]
    public void GivenValidParameters_WhenInstantiatingUnpublishedPostEntity_ThenSucceed()
    {
        var user = new User("username");
        var content = "content";
        var unpublishedPost = new UnpublishedPost(user, content);
        Assert.Equal(user, unpublishedPost.Author);
        Assert.Equal(content, unpublishedPost.Content);
    }

#pragma warning disable CS8604 // Possible null reference argument.
    [Fact]
    public void GivenNullAuthor_WhenInstantiatingUnpublishedPostEntity_ThenThrowException()
    {
        Assert.Throws<ArgumentNullException>(() => new UnpublishedPost(null, "content"));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void GivenEmptyPostContent_WhenInstantiatingUnpublishedPostEntity_ThenThrowException(string? content)
    {
        Assert.Throws<EmptyPostContentException>(() => new UnpublishedPost(new User("username"), content));
    }
#pragma warning restore CS8604 // Possible null reference argument.

    [Fact]
    public async Task GivenUserHasNotReachedMaxAllowedDailyPublications_WhenPublishingUnpublishedPost_ThenSucceed()
    {
        var user = new User("username");
        var content = "content";
        var unpublishedPost = new UnpublishedPost(user, content);
        var now = DateTime.UtcNow;

        // No, this test it not useless and is not testing only mockery.
        // Actually, it acts as a safeguard that can detect whenever the Publish method changes behavior.
        var persistencePort = A.Fake<IDomainPersistencePort>();
        A.CallTo(() => persistencePort.AmountOfPublicationsMadeTodayBy(user)).Returns(Task.FromResult<ushort>(0));
        A.CallTo(() => persistencePort.PublishNewPost(unpublishedPost)).Returns(Post
            .Builder()
            .WithId(1)
            .WithAuthor(user)
            .WithPublicationDate(now)
            .WithContent(content)
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
        var user = new User("username");
        var unpublishedPost = new UnpublishedPost(user, "content");

        var persistencePort = A.Fake<IDomainPersistencePort>();
        A.CallTo(() => persistencePort.AmountOfPublicationsMadeTodayBy(user))
            .Returns(UnpublishedPost.MAX_ALLOWED_DAILY_PUBLICATIONS_BY_USER);

        await Assert.ThrowsAsync<MaxAllowedDailyPublicationsByUserExceededException>(
            () => unpublishedPost.Publish(persistencePort)
        );
    }
}
