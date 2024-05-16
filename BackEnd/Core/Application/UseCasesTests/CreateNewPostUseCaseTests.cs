#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.

using Posterr.Core.Application.UseCases.Exceptions;
using Posterr.Core.Application.UseCases.CreateNewPost;
using Posterr.Core.Domain.Entities.Publications.Exceptions;

namespace Posterr.Core.Application.UseCaseTests;

public class CreateNewPostUseCaseTests
{
    private readonly PresumeThat presumeThat = PresumeThat.ItWorks();
    private readonly CreateNewPostUseCase useCase;

    public CreateNewPostUseCaseTests()
    {
        useCase = new CreateNewPostUseCase(presumeThat.UserRepository,
                                           presumeThat.DomainPersistenceAdapter,
                                           presumeThat.DomainConfig);
    }

    [Fact]
    public async Task GivenCorrectParametersAndRequirements_WhenCreatingNewPost_ThenSucceed()
    {
        var user = The.User(The.Username);
        var unpublishedPost = The.UnpublishedPost(user, The.Content);
        var post = The.Post(1, user, The.CurrentTimeUTC, unpublishedPost.Content);
        presumeThat.UserExists(user);
        presumeThat.UserHasNotMadePublicationsToday(user);
        presumeThat.DomainPersistencePortSuccessfullyPublishesPost(unpublishedPost, post);

        var request = new CreateNewPostRequest(user.Username, post.Content);
        var response = await useCase.Run(request);

        Assert.Equal(1, response.PostId);
        Assert.Equal(The.Username, response.AuthorUsername);
        Assert.Equal(The.CurrentTimeUTC, response.PublicationDate);
        Assert.Equal(The.Content, response.PostContent);
    }

    [Fact]
    public async Task GivenUserIsNotFound_WhenCreatingNewPost_ThenThrowException()
    {
        presumeThat.UserDoesNotExist(The.Username);
        var request = new CreateNewPostRequest(The.Username, The.Content);
        await Assert.ThrowsAsync<UserNotFoundException>(() => useCase.Run(request));
    }

    [Fact]
    public async Task GivenUserHasReachedMaxAllowedDailyPublications_WhenCreatingNewPost_ThenThrowException()
    {
        var user = The.User(The.Username);
        presumeThat.UserExists(user);
        presumeThat.UserHasReachedMaxAllowedDailyPublications(user);

        var request = new CreateNewPostRequest(user.Username, The.Content);

        await Assert.ThrowsAsync<MaxAllowedDailyPublicationsByUserExceededException>(() => useCase.Run(request));
    }

    [Fact]
    public void GivenNullUserRepository_WhenInstantiatingCreateNewPostUseCase_ThenThrowException() =>
        Assert.Throws<ArgumentNullException>(() =>
            new CreateNewPostUseCase(null, The.DomainPersistenceAdapter(), The.DomainConfigForTests()));

    [Fact]
    public void GivenNullDomainPersistenceAdapter_WhenInstantiatingCreateNewPostUseCase_ThenThrowException() =>
        Assert.Throws<ArgumentNullException>(() =>
            new CreateNewPostUseCase(The.UserRepository(), null, The.DomainConfigForTests()));

    [Fact]
    public void GivenNullDomainConfig_WhenInstantiatingCreateNewPostUseCase_ThenThrowException() =>
        Assert.Throws<ArgumentNullException>(() =>
            new CreateNewPostUseCase(The.UserRepository(), The.DomainPersistenceAdapter(), null));
}
