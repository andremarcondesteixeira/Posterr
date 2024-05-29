#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.

using FakeItEasy;
using Posterr.Core.Application.UseCases.CreateNewPost;
using Posterr.Core.Boundaries.Configuration;
using Posterr.Core.Boundaries.EntitiesInterfaces;
using Posterr.Core.Boundaries.Persistence;
using Posterr.Core.Domain.Entities;
using Posterr.Core.Shared.Exceptions;

namespace Posterr.Core.Application.UseCasesTests;

public class CreateNewPostUseCaseTests
{
    private readonly IUsersRepository usersRepository = Fake.UserRepository();
    private readonly IDomainPersistencePort domainPersistenceAdapter = Fake.DomainPersistenceAdapter();
    private readonly IDomainConfig domainConfig = Fake.DomainConfig();
    private readonly CreateNewPostUseCase useCase;

    public CreateNewPostUseCaseTests()
    {
        useCase = new CreateNewPostUseCase(usersRepository, domainPersistenceAdapter, domainConfig);
    }

    [Fact]
    public async Task GivenCorrectParametersAndRequirements_WhenCreatingNewPost_ThenSucceed()
    {
        var user = Fake.User(Fake.Username);
        var unpublishedPost = Fake.UnpublishedPost(user, Fake.Content);
        var post = Fake.Post(1, user, Fake.CurrentTimeUTC, unpublishedPost.Content);
        A.CallTo(() => usersRepository.FindByUsername(user.Username)).Returns(user);
        A.CallTo(() => domainPersistenceAdapter.AmountOfPublicationsMadeTodayBy(user)).Returns((ushort)0);
        A.CallTo(() => domainPersistenceAdapter.PublishNewPost(A<IUnpublishedPost>.That.Matches(
            x => x.Author.Username == unpublishedPost.Author.Username && x.Content == unpublishedPost.Content
        ))).Returns(post);

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
        A.CallTo(() => usersRepository.FindByUsername(Fake.Username)).Returns(Task.FromResult<IUser?>(null));
        var request = new CreateNewPostRequestDTO(Fake.Username, Fake.Content);
        await Assert.ThrowsAsync<UserNotFoundException>(() => useCase.Run(request));
    }

    [Fact]
    public async Task GivenUserHasReachedMaxAllowedDailyPublications_WhenCreatingNewPost_ThenThrowException()
    {
        var user = Fake.User(Fake.Username);
        A.CallTo(() => usersRepository.FindByUsername(user.Username)).Returns(user);
        A.CallTo(() => domainPersistenceAdapter.AmountOfPublicationsMadeTodayBy(user)).Returns(
            domainConfig.MaxAllowedDailyPublicationsByUser
        );

        var request = new CreateNewPostRequestDTO(user.Username, Fake.Content);

        await Assert.ThrowsAsync<MaxAllowedDailyPublicationsByUserExceededException>(() => useCase.Run(request));
    }
}
