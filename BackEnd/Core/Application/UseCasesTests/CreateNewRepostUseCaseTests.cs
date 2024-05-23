#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.

using Posterr.Core.Application.UseCases.Exceptions;
using Posterr.Core.Application.UseCases.CreateNewRepost;
using Posterr.Core.Domain.Entities.Publications.Exceptions;
using FakeItEasy;
using Posterr.Core.Boundaries.Persistence;
using Posterr.Core.Domain.Entities;
using Posterr.Core.Boundaries.Configuration;
using Posterr.Core.Boundaries.EntitiesInterfaces;

namespace Posterr.Core.Application.UseCasesTests;

public class CreateNewRepostUseCaseTests
{
    private readonly IUsersRepository usersRepository = Fake.UserRepository();
    private readonly IPublicationsRepository publicationsRepository = Fake.PublicationRepository();
    private readonly IDomainPersistencePort domainPersistenceAdapter = Fake.DomainPersistenceAdapter();
    private readonly IDomainConfig domainConfig = Fake.DomainConfig();
    private readonly CreateNewRepostUseCase useCase;

    public CreateNewRepostUseCaseTests()
    {
        useCase = new CreateNewRepostUseCase(usersRepository, publicationsRepository, domainPersistenceAdapter, domainConfig);
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
        A.CallTo(() => usersRepository.FindByUsername(repostAuthor.Username)).Returns(repostAuthor);
        A.CallTo(() => publicationsRepository.FindPostById(originalPost.Id)).Returns(originalPost);
        A.CallTo(() => domainPersistenceAdapter.AmountOfPublicationsMadeTodayBy(repostAuthor)).Returns((ushort)0);
        A.CallTo(() => domainPersistenceAdapter.PublishNewRepost(A<IUnpublishedRepost>.That.Matches(r =>
            r.Author.Username == unpublishedRepost.Author.Username
            && r.OriginalPost.Id == unpublishedRepost.OriginalPost.Id
            && r.OriginalPost.Author.Username == unpublishedRepost.OriginalPost.Author.Username
            && r.OriginalPost.PublicationDate == unpublishedRepost.OriginalPost.PublicationDate
            && r.OriginalPost.Content == unpublishedRepost.OriginalPost.Content
        ))).Returns(publishedRepost);

        var dto = new CreateNewRepostRequestDTO(repostAuthor.Username, originalPost.Id);
        var response = await useCase.Run(dto);

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
        A.CallTo(() => usersRepository.FindByUsername(Fake.Username)).Returns(Task.FromResult<IUser?>(null));
        var request = new CreateNewRepostRequestDTO(Fake.Username, 1);
        await Assert.ThrowsAsync<UserNotFoundException>(() => useCase.Run(request));
    }

    [Fact]
    public async Task GivenOriginalPostNotFound_WhenCreatingNewRepost_ThenThrowException()
    {
        var repostAuthor = Fake.User(Fake.Username);
        A.CallTo(() => usersRepository.FindByUsername(repostAuthor.Username)).Returns(repostAuthor);
        A.CallTo(() => publicationsRepository.FindPostById(1)).Returns(Task.FromResult<IPost?>(null));

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
        A.CallTo(() => usersRepository.FindByUsername(repostAuthor.Username)).Returns(repostAuthor);
        A.CallTo(() => publicationsRepository.FindPostById(originalPost.Id)).Returns(originalPost);
        A.CallTo(() => domainPersistenceAdapter.AmountOfPublicationsMadeTodayBy(repostAuthor)).Returns(
            domainConfig.MaxAllowedDailyPublicationsByUser
        );

        var createNewRepostRequest = new CreateNewRepostRequestDTO(repostAuthor.Username, originalPost.Id);

        await Assert.ThrowsAsync<MaxAllowedDailyPublicationsByUserExceededException>(
            () => useCase.Run(createNewRepostRequest)
        );
    }

    [Fact]
    public void GivenNullUserRepository_WhenInstantiatingCreateNewRepostUseCase_ThenThrowException()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            new CreateNewRepostUseCase(
                null,
                Fake.PublicationRepository(),
                Fake.DomainPersistenceAdapter(),
                Fake.DomainConfig()
            );
        });
    }

    [Fact]
    public void GivenNullPublicationRepository_WhenInstantiatingCreateNewRepostUseCase_ThenThrowException()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            new CreateNewRepostUseCase(
                Fake.UserRepository(),
                null,
                Fake.DomainPersistenceAdapter(),
                Fake.DomainConfig()
            );
        });
    }

    [Fact]
    public void GivenNullDomainPersistenceAdapter_WhenInstantiatingCreateNewRepostUseCase_ThenThrowException()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            new CreateNewRepostUseCase(
                Fake.UserRepository(),
                Fake.PublicationRepository(),
                null,
                Fake.DomainConfig()
            );
        });
    }

    [Fact]
    public void GivenNullDomainConfig_WhenInstantiatingCreateNewRepostUseCase_ThenThrowException()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            new CreateNewRepostUseCase(
                Fake.UserRepository(),
                Fake.PublicationRepository(),
                Fake.DomainPersistenceAdapter(),
                null
            );
        });
    }
}
