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
}
