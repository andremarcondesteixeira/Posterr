#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.

using FakeItEasy;
using Posterr.Core.Application.UseCases.CreateNewPost;
using Posterr.Core.Boundaries.ConfigurationInterface;
using Posterr.Core.Boundaries.EntitiesInterfaces;
using Posterr.Core.Boundaries.Persistence;
using Posterr.Core.Shared.Exceptions;

namespace Posterr.Core.Application.UseCasesTests;

public class CreateNewPostUseCaseTests
{
    private readonly IUsersRepository usersRepository = Fake.UserRepository();
    private readonly IPublicationsRepository publicationsRepository = Fake.PublicationsRepository();
    private readonly IDomainConfig domainConfig = Fake.DomainConfig();
    private readonly CreateNewPostUseCase useCase;

    public CreateNewPostUseCaseTests()
    {
        useCase = new CreateNewPostUseCase(usersRepository, publicationsRepository, domainConfig);
    }

    [Fact]
    public void GivenCorrectParametersAndRequirements_WhenCreatingNewPost_ThenSucceed()
    {
        var user = Fake.User(Fake.Username);
        var unpublishedPost = Fake.UnpublishedPost(user, Fake.Content);
        var post = Fake.Post(1, user, Fake.CurrentTimeUTC, unpublishedPost.Content);
        A.CallTo(() => usersRepository.FindByUsername(user.Username)).Returns(user);
        A.CallTo(() => publicationsRepository.CountPublicationsMadeByUserBetweenDateTimeRange(
            user,
            A<DateTime>.Ignored,
            A<DateTime>.Ignored)
        ).Returns(0);
        A.CallTo(() => publicationsRepository.PublishNewPost(A<IUnpublishedPost>.That.Matches(
            x => x.Author.Username == unpublishedPost.Author.Username && x.Content == unpublishedPost.Content
        ))).Returns(post);

        var request = new CreateNewPostUseCaseInputDTO(user.Username, post.Content);
        var response = useCase.Run(request);

        Assert.Equal(1, response.Id);
        Assert.Equal(Fake.Username, response.Author.Username);
        Assert.Equal(Fake.CurrentTimeUTC, response.PublicationDate);
        Assert.Equal(Fake.Content, response.Content);
    }

    [Fact]
    public void GivenUserIsNotFound_WhenCreatingNewPost_ThenThrowException()
    {
        A.CallTo(() => usersRepository.FindByUsername(Fake.Username)).Returns(null);
        var request = new CreateNewPostUseCaseInputDTO(Fake.Username, Fake.Content);
        Assert.Throws<UserNotFoundException>(() => useCase.Run(request));
    }

    [Fact]
    public void GivenUserHasReachedMaxAllowedDailyPublications_WhenCreatingNewPost_ThenThrowException()
    {
        var user = Fake.User(Fake.Username);
        A.CallTo(() => usersRepository.FindByUsername(user.Username)).Returns(user);
        A.CallTo(() => publicationsRepository.CountPublicationsMadeByUserBetweenDateTimeRange(
            user,
            A<DateTime>.Ignored,
            A<DateTime>.Ignored)
        ).Returns(
            domainConfig.MaxAllowedDailyPublicationsByUser
        );

        var request = new CreateNewPostUseCaseInputDTO(user.Username, Fake.Content);

        Assert.Throws<MaxAllowedDailyPublicationsByUserExceededException>(() => useCase.Run(request));
    }
}
