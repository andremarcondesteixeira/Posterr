using Posterr.Core.Domain.Publications;
using Posterr.Core.Domain.Publications.Exceptions;

namespace Posterr.Core.Domain.Tests.Publications;

public class PostContentTests
{
    [Fact]
    public void GivenValidContent_WhenInstantiatingPostContentValueObject_ThenSucceed()
    {
        var content = new PostContent("content");
        Assert.Equal("content", content.Value);
    }

#pragma warning disable CS8604 // Possible null reference argument.
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void GivenEmptyContent_WhenInstantiatingPostContentValueObject_ThenThrowException(string? content)
    {
        Assert.Throws<EmptyPostContentException>(() => new PostContent(content));
    }
#pragma warning restore CS8604 // Possible null reference argument.

    [Fact]
    public void GivenContentLengthIsGreaterThanMaxAllowedSize_WhenInstantiatingPostContentValueObject_ThenThrowException()
    {
        var content = new string('@', PostContent.MAX_LENGTH + 1);
        Assert.Throws<MaxPostContentLengthExceededException>(() => new PostContent(content));
    }
}
