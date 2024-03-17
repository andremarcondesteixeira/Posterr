using FakeItEasy;
using Posterr.Core.Application.CreateNewPost;
using Posterr.Core.Application.Interfaces;
using Posterr.Core.Domain;
using Posterr.Core.Domain.Publications;

namespace Posterr.Core.Application.Tests.CreateNewPost;

public class CreateNewPostUseCaseTests
{
    private readonly IUserRepository _userRepository;
    private readonly IDomainPersistencePort _domainPersistenceAdapter;
    private readonly IDomainConfig _domainConfig;

    public CreateNewPostUseCaseTests()
    {
        _userRepository = A.Fake<IUserRepository>();
        _domainPersistenceAdapter = A.Fake<IDomainPersistencePort>();

        _domainConfig = A.Fake<IDomainConfig>();
        A.CallTo(() => _domainConfig.MaxPostLength).Returns((uint) 7);
        A.CallTo(() => _domainConfig.MaxAllowedDailyPublicationsByUser).Returns((ushort) 5);
    }

    [Fact]
    public async Task GivenCorrectParametersAndRequirements_WhenCreatingNewPost_ThenSucceed()
    {
        string content = "content", username = "username";
        var now = DateTime.UtcNow;
        var user = Helpers.MakeDummyUser(username);
        var post = Helpers.MakeDummyPost(1, user, now, content);
        var unpublishedPost = Helpers.MakeDummyUnpublishedPost(user, content);
        A.CallTo(() => _userRepository.FindByUsername(username)).Returns(user);
        A.CallTo(() => _domainPersistenceAdapter.AmountOfPublicationsMadeTodayBy(user)).Returns((ushort) 0);
        A.CallTo(() => _domainPersistenceAdapter.PublishNewPost(
            A<IUnpublishedPost>.That.Matches(x => x.Author.Username == username && x.Content == content)
        )).Returns(post);

        var useCase = new CreateNewPostUseCase(_userRepository, _domainPersistenceAdapter, _domainConfig);
        var request = new CreateNewPostRequest(content, username);
        var response = await useCase.Run(request);

        Assert.Equal(1, response.PostId);
        Assert.Equal(username, response.AuthorUsername);
        Assert.Equal(now, response.PublicationDate);
        Assert.Equal(content, response.PostContent);
    }
}
