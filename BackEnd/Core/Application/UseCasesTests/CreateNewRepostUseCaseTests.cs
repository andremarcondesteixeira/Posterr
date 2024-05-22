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
        var yesterday = Fake.CurrentTimeUTC.AddDays(-1);
        var originalPostAuthor = Fake.User(Fake.OriginalPostAuthorUsername);
        var originalPost = Fake.Post(1, originalPostAuthor, yesterday, Fake.Content);
        var repostAuthor = Fake.User(Fake.RepostAuthorUsername);
        var unpublishedRepost = Fake.UnpublishedRepost(repostAuthor, originalPost);
        var publishedRepost = Fake.Repost(unpublishedRepost.Author, Fake.CurrentTimeUTC, unpublishedRepost.OriginalPost);
        presumeThat.UserExists(repostAuthor);
        presumeThat.UserHasNotMadePublicationsToday(repostAuthor);
        presumeThat.PostExists(originalPost);
        presumeThat.DomainPersistencePortSuccessfullyPublishesRepost(unpublishedRepost, publishedRepost);

        var createNewRepostRequest = new CreateNewRepostRequestDTO(repostAuthor.Username, originalPost.Id);
        var response = await useCase.Run(createNewRepostRequest);

        Assert.Equal(Fake.RepostAuthorUsername, response.RepostAuthorUsername);
        Assert.Equal(Fake.CurrentTimeUTC, response.RepostPublicationDate);
        Assert.Equal(1, response.OriginalPost.Id);
        Assert.Equal(Fake.OriginalPostAuthorUsername, response.OriginalPost.AuthorUsername);
        Assert.Equal(yesterday, response.OriginalPost.PublicationDate);
        Assert.Equal(Fake.Content, response.OriginalPost.Content);
    }

    [Fact]
    public async Task GivenRepostAuthorUsernameDoesNotBelongToAnyRegisteredUser_WhenCreatingNewRepost_ThenThrowException()
    {
        presumeThat.UserDoesNotExist(Fake.Username);
        var request = new CreateNewRepostRequestDTO(Fake.Username, 1);
        await Assert.ThrowsAsync<UserNotFoundException>(() => useCase.Run(request));
    }

    [Fact]
    public async Task GivenOriginalPostNotFound_WhenCreatingNewRepost_ThenThrowException()
    {
        var repostAuthor = Fake.User(Fake.Username);
        presumeThat.UserExists(repostAuthor);
        presumeThat.PostDoesNotExist(1);

        var request = new CreateNewRepostRequestDTO(repostAuthor.Username, 1);

        await Assert.ThrowsAsync<PostNotFoundException>(() => useCase.Run(request));
    }

    [Fact]
    public async Task GivenUserHasReachedMaxAllowedDailyPublications_WhenCreatingNewRepost_ThenThrowException()
    {
        var yesterday = Fake.CurrentTimeUTC.AddDays(-1);
        var originalPostAuthor = Fake.User(Fake.OriginalPostAuthorUsername);
        var originalPost = Fake.Post(1, originalPostAuthor, yesterday, Fake.Content);
        var repostAuthor = Fake.User(Fake.RepostAuthorUsername);
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
                                       Fake.PublicationRepository(),
                                       Fake.DomainPersistenceAdapter(),
                                       Fake.DomainConfigForTests()));

    [Fact]
    public void GivenNullPublicationRepository_WhenInstantiatingCreateNewRepostUseCase_ThenThrowException() =>
        Assert.Throws<ArgumentNullException>(() =>
            new CreateNewRepostUseCase(Fake.UserRepository(),
                                      null,
                                      Fake.DomainPersistenceAdapter(),
                                      Fake.DomainConfigForTests()));

    [Fact]
    public void GivenNullDomainPersistenceAdapter_WhenInstantiatingCreateNewRepostUseCase_ThenThrowException() =>
        Assert.Throws<ArgumentNullException>(() =>
            new CreateNewRepostUseCase(Fake.UserRepository(),
                                       Fake.PublicationRepository(),
                                       null,
                                       Fake.DomainConfigForTests()));

    [Fact]
    public void GivenNullDomainConfig_WhenInstantiatingCreateNewRepostUseCase_ThenThrowException() =>
        Assert.Throws<ArgumentNullException>(() =>
            new CreateNewRepostUseCase(Fake.UserRepository(),
                                       Fake.PublicationRepository(),
                                       Fake.DomainPersistenceAdapter(),
                                       null));
}
