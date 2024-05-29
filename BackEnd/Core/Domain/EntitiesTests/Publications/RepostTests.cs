#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.

using FakeItEasy;
using Posterr.Core.Boundaries.EntitiesInterfaces;
using Posterr.Core.Domain.Entities.Publications;

namespace Posterr.Core.Domain.EntitiesTests.Publications;

public class RepostTests
{
    [Fact]
    public void GivenValidArguments_WhenInstantiatingRepostEntity_ThenSucceed()
    {
        // Using fakes here future proofs the test against changes in the user and post models.
        // The same applies to other tests.
        var repostAuthor = A.Fake<IUser>();
        var originalPost = A.Fake<IPost>();
        var publicationDate = DateTime.UtcNow;
        var repost = new Repost(repostAuthor, originalPost, publicationDate);

        Assert.Equal(repostAuthor, repost.Author);
        Assert.Equal(originalPost, repost.OriginalPost);
        Assert.Equal(publicationDate, repost.PublicationDate);
    }

    [Fact]
    public void GivenNullAuthor_WhenInstantiatingRepostEntity_ThenThrowException()
    {
        Assert.Throws<ArgumentNullException>(() => new Repost(null, A.Fake<IPost>(), DateTime.UtcNow));
    }

    [Fact]
    public void GivenNullOriginalPost_WhenInstantiatingRepostEntity_ThenThrowException()
    {
        Assert.Throws<ArgumentNullException>(() => new Repost(A.Fake<IUser>(), null, DateTime.UtcNow));
    }
}
