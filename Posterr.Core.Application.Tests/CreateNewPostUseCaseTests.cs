using FakeItEasy;
using Posterr.Core.Application.CreateNewPost;
using Posterr.Core.Application.Exceptions;
using Posterr.Core.Application.Interfaces;
using Posterr.Core.Domain;
using Posterr.Core.Domain.Publications;
using Posterr.Core.Domain.Publications.Exceptions;
using Posterr.Core.Domain.Users;

namespace Posterr.Core.Application.Tests;

public class CreateNewPostUseCaseTests
{
    private const string USERNAME = "username";
    private const string CONTENT = "content";

    private readonly IUserRepository _userRepository;
    private readonly IDomainPersistencePort _domainPersistenceAdapter;
    private readonly IDomainConfig _domainConfig;

    public CreateNewPostUseCaseTests()
    {
        _userRepository = A.Fake<IUserRepository>();
        _domainPersistenceAdapter = A.Fake<IDomainPersistencePort>();

        _domainConfig = A.Fake<IDomainConfig>();
        A.CallTo(() => _domainConfig.MaxPostLength).Returns((uint)7);
        A.CallTo(() => _domainConfig.MaxAllowedDailyPublicationsByUser).Returns((ushort)5);
    }

    [Fact]
    public async Task GivenCorrectParametersAndRequirements_WhenCreatingNewPost_ThenSucceed()
    {
        var now = DateTime.UtcNow;
        var user = Helpers.MakeDummyUser(USERNAME);
        var post = Helpers.MakeDummyPost(1, user, now, CONTENT);
        var unpublishedPost = Helpers.MakeDummyUnpublishedPost(user, CONTENT);
        A.CallTo(() => _userRepository.FindByUsername(USERNAME)).Returns(user);
        A.CallTo(() => _domainPersistenceAdapter.AmountOfPublicationsMadeTodayBy(user)).Returns((ushort)0);
        A.CallTo(() => _domainPersistenceAdapter.PublishNewPost(
            A<IUnpublishedPost>.That.Matches(x => x.Author.Username == USERNAME && x.Content == CONTENT)
        )).Returns(post);

        var useCase = new CreateNewPostUseCase(_userRepository, _domainPersistenceAdapter, _domainConfig);
        var request = new CreateNewPostRequest(USERNAME, CONTENT);
        var response = await useCase.Run(request);

        Assert.Equal(1, response.PostId);
        Assert.Equal(USERNAME, response.AuthorUsername);
        Assert.Equal(now, response.PublicationDate);
        Assert.Equal(CONTENT, response.PostContent);
    }

    [Fact]
    public async Task GivenUserIsNotFound_WhenCreatingNewPost_ThenThrowException()
    {
        A.CallTo(() => _userRepository.FindByUsername(USERNAME)).Returns(Task.FromResult<IUser?>(null));

        var useCase = new CreateNewPostUseCase(_userRepository, _domainPersistenceAdapter, _domainConfig);
        var request = new CreateNewPostRequest(USERNAME, CONTENT);
        await Assert.ThrowsAsync<UserNotFoundException>(() => useCase.Run(request));
    }

    [Fact]
    public async Task GivenUserHasReachedMaxAllowedDailyPublications_WhenCreatingNewPost_ThenThrowException()
    {
        var user = Helpers.MakeDummyUser(USERNAME);
        A.CallTo(() => _userRepository.FindByUsername(USERNAME)).Returns(user);
        A.CallTo(() => _domainPersistenceAdapter.AmountOfPublicationsMadeTodayBy(user))
            .Returns(_domainConfig.MaxAllowedDailyPublicationsByUser);

        var useCase = new CreateNewPostUseCase(_userRepository, _domainPersistenceAdapter, _domainConfig);
        var request = new CreateNewPostRequest(USERNAME, CONTENT);
        await Assert.ThrowsAsync<MaxAllowedDailyPublicationsByUserExceededException>(() => useCase.Run(request));
    }

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.

    [Fact]
    public void GivenNullUserRepository_WhenInstantiatingCreateNewPostUseCase_ThenThrowException()
    {
        Assert.Throws<ArgumentNullException>(() => new CreateNewPostUseCase(null, _domainPersistenceAdapter, _domainConfig));
    }

    [Fact]
    public void GivenNullDomainPersistenceAdapter_WhenInstantiatingCreateNewPostUseCase_ThenThrowException()
    {
        Assert.Throws<ArgumentNullException>(() => new CreateNewPostUseCase(_userRepository, null, _domainConfig));
    }

    [Fact]
    public void GivenNullDomainConfig_WhenInstantiatingCreateNewPostUseCase_ThenThrowException()
    {
        Assert.Throws<ArgumentNullException>(() => new CreateNewPostUseCase(_userRepository, _domainPersistenceAdapter, null));
    }

#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
}
