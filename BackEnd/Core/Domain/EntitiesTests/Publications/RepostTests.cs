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
        var repost = new Repost(1, repostAuthor, publicationDate, "repost content", originalPost, Fake.DomainConfig());

        Assert.Equal(1, repost.Id);
        Assert.Equal(repostAuthor, repost.Author);
        Assert.Equal(publicationDate, repost.PublicationDate);
        Assert.Equal("repost content", repost.Content);
        Assert.Equal(originalPost, repost.OriginalPost);
    }

    [Fact]
    public void GivenNullAuthor_WhenInstantiatingRepostEntity_ThenThrowException()
    {
        Assert.Throws<ArgumentNullException>(() => new Repost(1, null, DateTime.UtcNow, "content", A.Fake<IPost>(), Fake.DomainConfig()));
    }

    [Fact]
    public void GivenNullOriginalPost_WhenInstantiatingRepostEntity_ThenThrowException()
    {
        Assert.Throws<ArgumentNullException>(() => new Repost(1, A.Fake<IUser>(), DateTime.UtcNow, "content", null, Fake.DomainConfig()));
    }

    [Fact]
    public void GivenNullDomainConfig_WhenInstantiatingRepostEntity_ThenThrowException()
    {
        Assert.Throws<ArgumentNullException>(() => new Repost(1, A.Fake<IUser>(), DateTime.UtcNow, "content", A.Fake<IPost>(), null));
    }
}
