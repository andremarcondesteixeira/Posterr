#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.

using Posterr.Core.Application.UseCases.Exceptions;
using Posterr.Core.Application.UseCases.CreateNewPost;
using Posterr.Core.Domain.Entities.Publications.Exceptions;

namespace Posterr.Core.Application.UseCasesTests;

public class CreateNewPostUseCaseTests
{
    private readonly Pretend pretend = Pretend.Make();
    private readonly CreateNewPostUseCase useCase;

    public CreateNewPostUseCaseTests()
    {
        useCase = new CreateNewPostUseCase(pretend.UserRepository,
                                           pretend.DomainPersistenceAdapter,
                                           pretend.DomainConfig);
    }

    [Fact]
    public async Task GivenCorrectParametersAndRequirements_WhenCreatingNewPost_ThenSucceed()
    {
        var user = Fake.User(Fake.Username);
        var unpublishedPost = Fake.UnpublishedPost(user, Fake.Content);
        var post = Fake.Post(1, user, Fake.CurrentTimeUTC, unpublishedPost.Content);
        pretend.FindUserByUsernameReturns(user);
        pretend.UserHasNotMadePublicationsToday(user);
        pretend.DomainPersistencePortSuccessfullyPublishesPost(unpublishedPost, post);

        var request = new CreateNewPostRequestDTO(user.Username, post.Content);
        var response = await useCase.Run(request);

        Assert.Equal(1, response.PostId);
        Assert.Equal(Fake.Username, response.AuthorUsername);
        Assert.Equal(Fake.CurrentTimeUTC, response.PublicationDate);
        Assert.Equal(Fake.Content, response.PostContent);
    }

    [Fact]
    public async Task GivenUserIsNotFound_WhenCreatingNewPost_ThenThrowException()
    {
        pretend.UserDoesNotExist(Fake.Username);
        var request = new CreateNewPostRequestDTO(Fake.Username, Fake.Content);
        await Assert.ThrowsAsync<UserNotFoundException>(() => useCase.Run(request));
    }

    [Fact]
    public async Task GivenUserHasReachedMaxAllowedDailyPublications_WhenCreatingNewPost_ThenThrowException()
    {
        var user = Fake.User(Fake.Username);
        pretend.FindUserByUsernameReturns(user);
        pretend.UserHasReachedMaxAllowedDailyPublications(user);

        var request = new CreateNewPostRequestDTO(user.Username, Fake.Content);

        await Assert.ThrowsAsync<MaxAllowedDailyPublicationsByUserExceededException>(() => useCase.Run(request));
    }

    [Fact]
    public void GivenNullUserRepository_WhenInstantiatingCreateNewPostUseCase_ThenThrowException() =>
        Assert.Throws<ArgumentNullException>(() =>
            new CreateNewPostUseCase(null, Fake.DomainPersistenceAdapter(), Fake.DomainConfig()));

    [Fact]
    public void GivenNullDomainPersistenceAdapter_WhenInstantiatingCreateNewPostUseCase_ThenThrowException() =>
        Assert.Throws<ArgumentNullException>(() =>
            new CreateNewPostUseCase(Fake.UserRepository(), null, Fake.DomainConfig()));

    [Fact]
    public void GivenNullDomainConfig_WhenInstantiatingCreateNewPostUseCase_ThenThrowException() =>
        Assert.Throws<ArgumentNullException>(() =>
            new CreateNewPostUseCase(Fake.UserRepository(), Fake.DomainPersistenceAdapter(), null));
}
