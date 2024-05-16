using FakeItEasy;
using Posterr.Core.Boundaries.Configuration;
using Posterr.Core.Boundaries.EntitiesInterfaces;
using Posterr.Core.Domain.Entities.Publications;
using Posterr.Core.Domain.Entities.Publications.Exceptions;

namespace Posterr.Core.Domain.EntitiesTests.Publications;

public class PostTests
{
    private readonly IDomainConfig _domainConfig;

    public PostTests()
    {
        _domainConfig = A.Fake<IDomainConfig>();
        A.CallTo(() => _domainConfig.MaxPostLength).Returns((uint) 4);
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
        Assert.Throws<EmptyPostContentException>(() => Post
            .Builder()
            .WithId(1)
            .WithAuthor(A.Fake<IUser>())
            .WithPublicationDate(DateTime.UtcNow)
            .WithContent(content)
            .WithDomainConfig(_domainConfig)
            .Build()
        );
    }

    [Fact]
    public void GivenContentLengthIsGreaterThanMaximumAllowed_WhenInstantiatingPostEntity_ThenThrowException()
    {
        var content = new string('@', (int) _domainConfig.MaxPostLength + 1);
        Assert.Throws<MaxPostContentLengthExceededException>(() => Post
            .Builder()
            .WithId(1)
            .WithAuthor(A.Fake<IUser>())
            .WithPublicationDate(DateTime.UtcNow)
            .WithContent(content)
            .WithDomainConfig(_domainConfig)
            .Build()
        );
    }

    [Theory]
    [InlineData(true, true, true, true, true)]
    [InlineData(true, true, true, true, false)]
    [InlineData(true, true, true, false, true)]
    [InlineData(true, true, true, false, false)]
    [InlineData(true, true, false, true, true)]
    [InlineData(true, true, false, true, false)]
    [InlineData(true, true, false, false, true)]
    [InlineData(true, true, false, false, false)]
    [InlineData(true, false, true, true, true)]
    [InlineData(true, false, true, true, false)]
    [InlineData(true, false, true, false, true)]
    [InlineData(true, false, true, false, false)]
    [InlineData(true, false, false, true, true)]
    [InlineData(true, false, false, true, false)]
    [InlineData(true, false, false, false, true)]
    [InlineData(true, false, false, false, false)]
    [InlineData(false, true, true, true, true)]
    [InlineData(false, true, true, true, false)]
    [InlineData(false, true, true, false, true)]
    [InlineData(false, true, true, false, false)]
    [InlineData(false, true, false, true, true)]
    [InlineData(false, true, false, true, false)]
    [InlineData(false, true, false, false, true)]
    [InlineData(false, true, false, false, false)]
    [InlineData(false, false, true, true, true)]
    [InlineData(false, false, true, true, false)]
    [InlineData(false, false, true, false, true)]
    [InlineData(false, false, true, false, false)]
    [InlineData(false, false, false, true, true)]
    [InlineData(false, false, false, true, false)]
    [InlineData(false, false, false, false, true)]
    public void GivenAnyNullParameter_WhenInstantiatingPostEntity_ThenThrowException(
        bool makeIdNull,
        bool makeAuthorNull,
        bool makePublicationDateNull,
        bool makeContentNull,
        bool makeDomainConfigNull
    )
    {
        List<string> expectedListOfPropsWitNullValue = [];

        var exception = Assert.Throws<PostBuilderStateHadNullValuesOnBuildException>(() =>
        {
            var postBuilder = Post.Builder();

            if (makeIdNull)
            {
                expectedListOfPropsWitNullValue.Add("Id");
            }
            else
            {
                postBuilder = postBuilder.WithId(1);
            }

            if (makeAuthorNull)
            {
                expectedListOfPropsWitNullValue.Add("Author");
            }
            else
            {
                postBuilder = postBuilder.WithAuthor(A.Fake<IUser>());
            }
            
            if (makePublicationDateNull)
            {
                expectedListOfPropsWitNullValue.Add("PublicationDate");
            }
            else
            {
                postBuilder = postBuilder.WithPublicationDate(DateTime.UtcNow);
            }

            if (makeContentNull)
            {
                expectedListOfPropsWitNullValue.Add("Content");
            }
            else
            {
                postBuilder = postBuilder.WithContent("content");
            }

            if (makeDomainConfigNull)
            {
                expectedListOfPropsWitNullValue.Add("DomainConfig");
            }
            else
            {
                postBuilder = postBuilder.WithDomainConfig(A.Fake<IDomainConfig>());
            }

            return postBuilder.Build();
        });


        Assert.Equal(expectedListOfPropsWitNullValue, exception.PropertiesWithNullValue);
    }
}
