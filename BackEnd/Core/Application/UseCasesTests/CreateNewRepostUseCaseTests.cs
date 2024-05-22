#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.

using Posterr.Core.Application.UseCases.Exceptions;
using Posterr.Core.Application.UseCases.CreateNewRepost;
using Posterr.Core.Domain.Entities.Publications.Exceptions;

namespace Posterr.Core.Application.UseCasesTests;

public class CreateNewRepostUseCaseTests
{
    private readonly PresumeThat presumeThat = PresumeThat.ItWorks();
    private readonly CreateNewRepostUseCase useCase;

    public CreateNewRepostUseCaseTests()
    {
        useCase = new CreateNewRepostUseCase(presumeThat.UserRepository,
                                              presumeThat.PublicationRepository,
                                              presumeThat.DomainPersistenceAdapter,
                                              presumeThat.DomainConfig);
    }

    [Fact]
    public async Task GivenCorrectParametersAndRequirements_WhenCreatingNewRepost_ThenSucceed()
    {
        var yesterday = The.CurrentTimeUTC.AddDays(-1);
        var originalPostAuthor = The.User(The.OriginalPostAuthorUsername);
        var originalPost = The.Post(1, originalPostAuthor, yesterday, The.Content);
        var repostAuthor = The.User(The.RepostAuthorUsername);
        var unpublishedRepost = The.UnpublishedRepost(repostAuthor, originalPost);
        var publishedRepost = The.Repost(unpublishedRepost.Author, The.CurrentTimeUTC, unpublishedRepost.OriginalPost);
        presumeThat.UserExists(repostAuthor);
        presumeThat.UserHasNotMadePublicationsToday(repostAuthor);
        presumeThat.PostExists(originalPost);
        presumeThat.DomainPersistencePortSuccessfullyPublishesRepost(unpublishedRepost, publishedRepost);

        var createNewRepostRequest = new CreateNewRepostRequestDTO(repostAuthor.Username, originalPost.Id);
        var response = await useCase.Run(createNewRepostRequest);

        Assert.Equal(The.RepostAuthorUsername, response.RepostAuthorUsername);
        Assert.Equal(The.CurrentTimeUTC, response.RepostPublicationDate);
        Assert.Equal(1, response.OriginalPost.Id);
        Assert.Equal(The.OriginalPostAuthorUsername, response.OriginalPost.AuthorUsername);
        Assert.Equal(yesterday, response.OriginalPost.PublicationDate);
        Assert.Equal(The.Content, response.OriginalPost.Content);
    }

    [Fact]
    public async Task GivenRepostAuthorUsernameDoesNotBelongToAnyRegisteredUser_WhenCreatingNewRepost_ThenThrowException()
    {
        presumeThat.UserDoesNotExist(The.Username);
        var request = new CreateNewRepostRequestDTO(The.Username, 1);
        await Assert.ThrowsAsync<UserNotFoundException>(() => useCase.Run(request));
    }

    [Fact]
    public async Task GivenOriginalPostNotFound_WhenCreatingNewRepost_ThenThrowException()
    {
        var repostAuthor = The.User(The.Username);
        presumeThat.UserExists(repostAuthor);
        presumeThat.PostDoesNotExist(1);

        var request = new CreateNewRepostRequestDTO(repostAuthor.Username, 1);

        await Assert.ThrowsAsync<PostNotFoundException>(() => useCase.Run(request));
    }

    [Fact]
    public async Task GivenUserHasReachedMaxAllowedDailyPublications_WhenCreatingNewRepost_ThenThrowException()
    {
        var yesterday = The.CurrentTimeUTC.AddDays(-1);
        var originalPostAuthor = The.User(The.OriginalPostAuthorUsername);
        var originalPost = The.Post(1, originalPostAuthor, yesterday, The.Content);
        var repostAuthor = The.User(The.RepostAuthorUsername);
        presumeThat.UserExists(repostAuthor);
        presumeThat.PostExists(originalPost);
        presumeThat.UserHasReachedMaxAllowedDailyPublications(repostAuthor);

        var createNewRepostRequest = new CreateNewRepostRequestDTO(repostAuthor.Username, originalPost.Id);

        await Assert.ThrowsAsync<MaxAllowedDailyPublicationsByUserExceededException>(() =>
            useCase.Run(createNewRepostRequest)
        );
    }

    [Fact]
    public void GivenNullUserRepository_WhenInstantiatingCreateNewRepostUseCase_ThenThrowException() =>
        Assert.Throws<ArgumentNullException>(() =>
            new CreateNewRepostUseCase(null,
                                       The.PublicationRepository(),
                                       The.DomainPersistenceAdapter(),
                                       The.DomainConfigForTests()));

    [Fact]
    public void GivenNullPublicationRepository_WhenInstantiatingCreateNewRepostUseCase_ThenThrowException() =>
        Assert.Throws<ArgumentNullException>(() =>
            new CreateNewRepostUseCase(The.UserRepository(),
                                      null,
                                      The.DomainPersistenceAdapter(),
                                      The.DomainConfigForTests()));

    [Fact]
    public void GivenNullDomainPersistenceAdapter_WhenInstantiatingCreateNewRepostUseCase_ThenThrowException() =>
        Assert.Throws<ArgumentNullException>(() =>
            new CreateNewRepostUseCase(The.UserRepository(),
                                       The.PublicationRepository(),
                                       null,
                                       The.DomainConfigForTests()));

    [Fact]
    public void GivenNullDomainConfig_WhenInstantiatingCreateNewRepostUseCase_ThenThrowException() =>
        Assert.Throws<ArgumentNullException>(() =>
            new CreateNewRepostUseCase(The.UserRepository(),
                                       The.PublicationRepository(),
                                       The.DomainPersistenceAdapter(),
                                       null));
}
