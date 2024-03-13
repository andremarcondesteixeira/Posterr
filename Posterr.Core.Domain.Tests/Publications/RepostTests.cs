using Posterr.Core.Domain.Publications;
using Posterr.Core.Domain.Users;

namespace Posterr.Core.Domain.Tests.Publications;

// TODO: Replace occurrences of new User and new Post with A.Fake<>() by introducing an interface, which can be mocked.
// Currently, sealed records cannot be mocked.
public class RepostTests
{
    [Fact]
    public void GivenValidArguments_WhenCreatingRepostEntity_ThenSucceed()
    {
        var repostAuthor = new User("username");
        var originalPost = Post.Builder()
            .WithId(1)
            .WithAuthor(new User("originalAuthor"))
            .WithPublicationDate(DateTime.UtcNow)
            .WithContent("content")
            .Build();
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
        var originalPost = Post.Builder()
            .WithId(1)
            .WithAuthor(new User("originalAuthor"))
            .WithPublicationDate(DateTime.UtcNow)
            .WithContent("content")
            .Build();
        Assert.Throws<ArgumentNullException>(() => new Repost(null, originalPost, DateTime.UtcNow));
    }

    [Fact]
    public void GivenNullOriginalPost_WhenInstantiatingRepostEntity_ThenThrowException()
    {
        Assert.Throws<ArgumentNullException>(() => new Repost(new User("username"), null, DateTime.UtcNow));
    }
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
}
