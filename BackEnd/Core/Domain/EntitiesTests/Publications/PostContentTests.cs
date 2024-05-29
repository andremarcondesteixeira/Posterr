using FakeItEasy;
using Posterr.Core.Boundaries.Configuration;
using Posterr.Core.Domain.Entities.Publications;
using Posterr.Core.Shared.Exceptions;

namespace Posterr.Core.Domain.EntitiesTests.Publications;

public class PostContentTests
{
    private readonly IDomainConfig _domainConfig;

    public PostContentTests()
    {
        _domainConfig = A.Fake<IDomainConfig>();
        A.CallTo(() => _domainConfig.MaxPostLength).Returns((uint) 7);
    }

    [Fact]
    public void GivenValidContent_WhenInstantiatingPostContentValueObject_ThenSucceed()
    {
        var content = new PostContent("content", _domainConfig);
        Assert.Equal("content", content.Value);
    }

#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
    [Fact]
    public void GivenNullDomainConfig_WhenInstantiatingPostContentValueObject_ThenThrowException()
    {
        Assert.Throws<ArgumentNullException>(() => new PostContent("content", null));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void GivenEmptyContent_WhenInstantiatingPostContentValueObject_ThenThrowException(string? content)
    {
        Assert.Throws<EmptyPostContentException>(() => new PostContent(content, _domainConfig));
    }
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

    [Fact]
    public void GivenContentLengthIsGreaterThanMaxAllowedSize_WhenInstantiatingPostContentValueObject_ThenThrowException()
    {
        var content = new string('@', (int) _domainConfig.MaxPostLength + 1);
        Assert.Throws<MaxPostContentLengthExceededException>(() => new PostContent(content, _domainConfig));
    }
}
