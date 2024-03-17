using FakeItEasy;
using Posterr.Core.Application.CreateNewPost;
using Posterr.Core.Application.Interfaces;
using Posterr.Core.Domain;
using Posterr.Core.Domain.Publications;
using Posterr.Core.Domain.Users;

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
        var content = "content";
        var username = "username";
        var now = DateTime.UtcNow;
        var user = MakeDummyUser(username);
        var post = MakeDummyPost(1, user, now, content);
        var unpublishedPost = MakeDummyUnpublishedPost(user, content);
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

    private static IUser MakeDummyUser(string username)
    {
        var user = A.Fake<IUser>();
        A.CallTo(() => user.Username).Returns(username);
        return user;
    }

    private static IPost MakeDummyPost(long postId, IUser user, DateTime now, string content)
    {
        var post = A.Fake<IPost>();

        A.CallTo(() => post.Id).Returns(postId);
        A.CallTo(() => post.Author).Returns(user);
        A.CallTo(() => post.PublicationDate).Returns(now);
        A.CallTo(() => post.Content).Returns(content);

        return post;
    }

    private static IUnpublishedPost MakeDummyUnpublishedPost(IUser author, string content)
    {
        var unpublishedPost = A.Fake<IUnpublishedPost>();

        A.CallTo(() => unpublishedPost.Author).Returns(author);
        A.CallTo(() => unpublishedPost.Content).Returns(content);
        
        return unpublishedPost;
    }
}
