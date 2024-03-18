using FakeItEasy;
using Posterr.Core.Domain.Boundaries.Persistence;
using Posterr.Core.Domain.Publications;

namespace Posterr.Core.Domain.Tests.Publications;

// TODO: Replace occurrences of new User and new Post with A.Fake<>() by introducing an interface, which can be mocked.
// Currently, sealed records cannot be mocked.
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

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
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
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
}
