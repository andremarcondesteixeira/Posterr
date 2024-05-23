using FakeItEasy;
using Posterr.Core.Application.UseCases.PaginatePublications;
using Posterr.Core.Boundaries.EntitiesInterfaces;
using Posterr.Core.Boundaries.Persistence;

namespace Posterr.Core.Application.UseCasesTests;

public class PaginatePublicationsUseCaseTests
{
    private readonly PaginatePublicationsUseCase useCase;
    private readonly IPublicationsRepository publicationsRepository = Fake.PublicationsRepository();

    public PaginatePublicationsUseCaseTests()
    {
        useCase = new PaginatePublicationsUseCase(publicationsRepository);
    }

    [Fact]
    public async Task GivenAPageHasPostsAndReposts_ThenRepostsShouldBeFlagged()
    {
        IUser posterUser = Fake.User(Fake.OriginalPostAuthorUsername);
        IUser reposterUser = Fake.User(Fake.RepostAuthorUsername);
        var yesterday = DateTime.UtcNow.AddDays(-1);
        var now = DateTime.UtcNow;
        IPost post = Fake.Post(1, posterUser, yesterday, Fake.Content);
        IRepost repost = Fake.Repost(reposterUser, now, post);
        A.CallTo(() => publicationsRepository.Paginate(0, 15)).Returns([repost, post]);

        IList<PaginatePublicationsResponseItemDTO> publications =
            await useCase.Run(new PaginatePublicationsRequestDTO(1, 15));

        Assert.Equal(2, publications.Count);

        Assert.True(publications[0].IsRepost);
        Assert.Equal(1, publications[0].Post.PostId);
        Assert.Equal(Fake.OriginalPostAuthorUsername, publications[0].Post.AuthorUsername);
        Assert.Equal(yesterday, publications[0].Post.PublicationDate);
        Assert.Equal(Fake.Content, publications[0].Post.Content);
        Assert.Equal(Fake.RepostAuthorUsername, publications[0].Repost!.AuthorUsername);
        Assert.Equal(now, publications[0].Repost!.PublicationDate);

        Assert.False(publications[1].IsRepost);
        Assert.Equal(1, publications[1].Post.PostId);
        Assert.Equal(Fake.OriginalPostAuthorUsername, publications[1].Post.AuthorUsername);
        Assert.Equal(yesterday, publications[1].Post.PublicationDate);
        Assert.Equal(Fake.Content, publications[1].Post.Content);
        Assert.Null(publications[1].Repost);
    }
}