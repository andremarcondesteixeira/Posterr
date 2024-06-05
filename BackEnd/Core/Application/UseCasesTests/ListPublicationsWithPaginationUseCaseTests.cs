using FakeItEasy;
using Posterr.Core.Application.UseCases.ListPublicationsWithPagination;
using Posterr.Core.Boundaries.ConfigurationInterface;
using Posterr.Core.Boundaries.EntitiesInterfaces;
using Posterr.Core.Boundaries.Persistence;
using Posterr.Core.Shared.Exceptions;

namespace Posterr.Core.Application.UseCasesTests;

public class ListPublicationsWithPaginationUseCaseTests
{
    private readonly ListPublicationsUseCase useCase;
    private readonly IPublicationsRepository publicationsRepository = Fake.PublicationsRepository();
    private readonly IDomainConfig domainConfig = Fake.DomainConfig();

    public ListPublicationsWithPaginationUseCaseTests()
    {
        useCase = new(publicationsRepository);
    }

    [Fact]
    public void GivenAPageHasPostsAndReposts_ThenRepostsShouldBeFlagged()
    {
        var yesterday = DateTime.UtcNow.AddDays(-1);
        IUser posterUser = Fake.User(Fake.OriginalPostAuthorUsername);
        IPost post = Fake.Post(1, posterUser, yesterday, Fake.Content);
        
        var now = DateTime.UtcNow;
        IUser reposterUser = Fake.User(Fake.RepostAuthorUsername);
        IRepost repost = Fake.Repost(2, reposterUser, now, "repost content", post);
        
        A.CallTo(() => publicationsRepository.Paginate(0, 15)).Returns([repost, post]);

        IList<IPublication> publications = useCase.Run(new ListPublicationsUseCaseInputDTO(1, domainConfig));

        Assert.Equal(2, publications.Count);

        Assert.True(publications[0] is IRepost);
        Assert.Equal(2, publications[0].Id);
        Assert.Equal(Fake.RepostAuthorUsername, publications[0].Author.Username);
        Assert.Equal(now, publications[0].PublicationDate);
        Assert.Equal("repost content", publications[0].Content);
        Assert.Equal(Fake.OriginalPostAuthorUsername, ((IRepost)publications[0]).OriginalPost.Author.Username);
        Assert.Equal(yesterday, ((IRepost)publications[0]).OriginalPost.PublicationDate);

        Assert.False(publications[1] is IRepost);
        Assert.Equal(1, publications[1].Id);
        Assert.Equal(Fake.OriginalPostAuthorUsername, publications[1].Author.Username);
        Assert.Equal(yesterday, publications[1].PublicationDate);
        Assert.Equal(Fake.Content, publications[1].Content);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void GivenAnInvalidPageNumber_ThenThrowException(int pageNumber)
    {
        Assert.Throws<InvalidPageNumberException>(
            () => useCase.Run(new ListPublicationsUseCaseInputDTO(pageNumber, domainConfig))
        );
    }
}