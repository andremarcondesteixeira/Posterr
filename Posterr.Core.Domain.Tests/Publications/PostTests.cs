using Posterr.Core.Domain.Publications;
using Posterr.Core.Domain.Publications.Exceptions;
using Posterr.Core.Domain.Users;

namespace Posterr.Core.Domain.Tests.Publications;

public class PostTests
{
    [Fact]
    public void GivenValidParameters_WhenInstantiatingPostEntity_ThenSucceed()
    {
        // TODO: Future proof this test by creating an interface that can be mocked. User is sealed and cannot be mocked.
        var author = new User("username");
        var publicationDate = DateTime.UtcNow;
        var content = "test";
        var post = Post.Builder()
                .WithId(1)
                .WithAuthor(author)
                .WithPublicationDate(publicationDate)
                .WithContent(content)
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
            // TODO: Future proof this test by creating an interface that can be mocked. User is sealed and cannot be mocked.
            .WithAuthor(new User("username"))
            .WithPublicationDate(DateTime.UtcNow)
            .WithContent(content)
            .Build()
        );
    }

    [Fact]
    public void GivenContentLengthIsGreaterThanMaximumAllowed_WhenInstantiatingPostEntity_ThenThrowException()
    {
        var content = new string('@', PostContent.MAX_LENGTH + 1);
        Assert.Throws<MaxPostContentLengthExceededException>(() => Post
            .Builder()
            .WithId(1)
            // TODO: Future proof this test by creating an interface that can be mocked. User is sealed and cannot be mocked.
            .WithAuthor(new User("username"))
            .WithPublicationDate(DateTime.UtcNow)
            .WithContent(content)
            .Build()
        );
    }

#pragma warning disable CS8604 // Possible null reference argument.
    [Theory]
    [InlineData(true, true, true, true)]
    [InlineData(true, true, true, false)]
    [InlineData(true, true, false, true)]
    [InlineData(true, true, false, false)]
    [InlineData(true, false, true, true)]
    [InlineData(true, false, true, false)]
    [InlineData(true, false, false, true)]
    [InlineData(true, false, false, false)]
    [InlineData(false, true, true, true)]
    [InlineData(false, true, true, false)]
    [InlineData(false, true, false, true)]
    [InlineData(false, true, false, false)]
    [InlineData(false, false, true, true)]
    [InlineData(false, false, true, false)]
    [InlineData(false, false, false, true)]
    public void GivenAnyNullParameter_WhenInstantiatingPostEntity_ThenThrowException(
        bool makeIdNull,
        bool makeAuthorNull,
        bool makePublicationDateNull,
        bool makeContentNull
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
                // TODO: Future proof this test by creating an interface that can be mocked. User is sealed and cannot be mocked.
                postBuilder = postBuilder.WithAuthor(new User("username"));
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

            return postBuilder.Build();
        });


        Assert.Equal(expectedListOfPropsWitNullValue, exception.PropertiesWithNullValue);
    }
#pragma warning restore CS8604 // Possible null reference argument.
}
