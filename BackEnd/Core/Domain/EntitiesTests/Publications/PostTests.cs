using FakeItEasy;
using Posterr.Core.Domain.Entities.Publications;
using Posterr.Core.Shared.ConfigurationInterfaces;
using Posterr.Core.Shared.EntitiesInterfaces;
using Posterr.Core.Shared.Exceptions;

namespace Posterr.Core.Domain.EntitiesTests.Publications;

public class PostTests
{
    private readonly IDomainConfig _domainConfig;

    public PostTests()
    {
        _domainConfig = A.Fake<IDomainConfig>();
        A.CallTo(() => _domainConfig.MaxPostLength).Returns((uint)4);
    }

    [Fact]
    public void GivenValidParameters_WhenInstantiatingPostEntity_ThenSucceed()
    {
        // Using A.Fake for such a simple object provides value because
        // this code will not need change when new properties are added to IUser
        var author = A.Fake<IUser>();
        var publicationDate = DateTime.UtcNow;
        var content = "test";
        var post = Post.Builder()
            .WithId(1)
            .WithAuthor(author)
            .WithPublicationDate(publicationDate)
            .WithContent(content)
            .WithDomainConfig(_domainConfig)
            .Build();

        Assert.Equal(1, post.Id);
        Assert.Equal(author, post.Author);
        Assert.Equal(content, post.Content);
        Assert.Equal(publicationDate, post.PublicationDate);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void GivenEmptyContent_WhenInstantiatingPostEntity_ThenThrowException(string content)
    {
        Assert.Throws<EmptyPostContentException>(() =>
        {
            Post.Builder()
                .WithId(1)
                .WithAuthor(A.Fake<IUser>())
                .WithPublicationDate(DateTime.UtcNow)
                .WithContent(content)
                .WithDomainConfig(_domainConfig)
                .Build();
        });
    }

    [Fact]
    public void GivenContentLengthIsGreaterThanMaximumAllowed_WhenInstantiatingPostEntity_ThenThrowException()
    {
        var content = new string('@', (int)_domainConfig.MaxPostLength + 1);
        Assert.Throws<MaxPublicationContentLengthExceededException>(() =>
        {
            Post.Builder()
                .WithId(1)
                .WithAuthor(A.Fake<IUser>())
                .WithPublicationDate(DateTime.UtcNow)
                .WithContent(content)
                .WithDomainConfig(_domainConfig)
                .Build();
        });
    }

    [Fact]
    public void GivenAnyNullParameter_WhenInstantiatingPostEntity_ThenThrowException()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            Post.Builder()
                .WithAuthor(A.Fake<IUser>())
                .WithPublicationDate(DateTime.UtcNow)
                .WithContent("content")
                .WithDomainConfig(A.Fake<IDomainConfig>())
                .Build();
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            Post.Builder()
                .WithId(1)
                .WithPublicationDate(DateTime.UtcNow)
                .WithContent("content")
                .WithDomainConfig(A.Fake<IDomainConfig>())
                .Build();
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            Post.Builder()
                .WithId(1)
                .WithAuthor(A.Fake<IUser>())
                .WithContent("content")
                .WithDomainConfig(A.Fake<IDomainConfig>())
                .Build();
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            Post.Builder()
                .WithId(1)
                .WithAuthor(A.Fake<IUser>())
                .WithPublicationDate(DateTime.UtcNow)
                .WithDomainConfig(A.Fake<IDomainConfig>())
                .Build();
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            Post.Builder()
                .WithId(1)
                .WithAuthor(A.Fake<IUser>())
                .WithPublicationDate(DateTime.UtcNow)
                .WithContent("content")
                .Build();
        });
    }
}
