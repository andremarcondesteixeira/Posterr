using FakeItEasy;
using Posterr.Core.Application.CreateNewRepost;
using Posterr.Core.Application.Interfaces;
using Posterr.Core.Domain;
using Posterr.Core.Domain.Publications;

namespace Posterr.Core.Application.Tests;

public class CreateNewRepostUseCaseTests
{
    private const string REPOST_AUTHOR_USERNAME = "repost_author_username";
    private const string ORIGINAL_POST_AUTHOR_USERNAME = "original_post_author_username";
    private const string CONTENT = "content";
    private readonly DateTime NOW = DateTime.UtcNow;

    private readonly IUserRepository _userRepository;
    private readonly IPublicationRepository _publicationRepository;
    private readonly IDomainPersistencePort _domainPersistenceAdapter;
    private readonly IDomainConfig _domainConfig;

    public CreateNewRepostUseCaseTests()
    {
        _userRepository = A.Fake<IUserRepository>(x => x.Strict());
        _publicationRepository = A.Fake<IPublicationRepository>(x => x.Strict());
        _domainPersistenceAdapter = A.Fake<IDomainPersistencePort>(x => x.Strict());

        _domainConfig = A.Fake<IDomainConfig>();
        A.CallTo(() => _domainConfig.MaxPostLength).Returns((uint)CONTENT.Length);
        A.CallTo(() => _domainConfig.MaxAllowedDailyPublicationsByUser).Returns((ushort)5);
    }

    [Fact]
    public async Task GivenCorrectParametersAndRequirements_WhenCreatingNewRepost_ThenSucceed()
    {
        var originalPostAuthor = Helpers.MakeDummyUser(ORIGINAL_POST_AUTHOR_USERNAME);
        var originalPost = Helpers.MakeDummyPost(1, originalPostAuthor, NOW.AddDays(-1), CONTENT);
        var repostAuthor = Helpers.MakeDummyUser(REPOST_AUTHOR_USERNAME);
        var createNewRepostRequest = new CreateNewRepostRequest(repostAuthor.Username, originalPost.Id);
        var publishedRepost = Helpers.MakeDummyRepost(repostAuthor, NOW, originalPost);

        A.CallTo(() => _userRepository.FindByUsername(createNewRepostRequest.AuthorUsername)).Returns(repostAuthor);
        A.CallTo(() => _publicationRepository.FindPostById(originalPost.Id)).Returns(originalPost);
        A.CallTo(() => _domainPersistenceAdapter.AmountOfPublicationsMadeTodayBy(repostAuthor)).Returns((ushort)0);
        A.CallTo(() => _domainPersistenceAdapter.PublishNewRepost(
            A<IUnpublishedRepost>.That.Matches(r =>
                r.Author.Username == createNewRepostRequest.AuthorUsername
                && r.OriginalPost.Id == originalPost.Id
                && r.OriginalPost.Author.Username == originalPost.Author.Username
                && r.OriginalPost.PublicationDate == originalPost.PublicationDate
                && r.OriginalPost.Content == originalPost.Content
            )
        )).Returns(publishedRepost);

        var useCase = new CreateNewRepostUseCase(
            _userRepository,
            _publicationRepository,
            _domainPersistenceAdapter,
            _domainConfig
        );
        var response = await useCase.Run(createNewRepostRequest);

        Assert.Equal(REPOST_AUTHOR_USERNAME, response.RepostAuthorUsername);
        Assert.Equal(NOW, response.RepostPublicationDate);
        Assert.Equal(1, response.OriginalPost.Id);
        Assert.Equal(ORIGINAL_POST_AUTHOR_USERNAME, response.OriginalPost.AuthorUsername);
        Assert.Equal(NOW.AddDays(-1), response.OriginalPost.PublicationDate);
        Assert.Equal(CONTENT, response.OriginalPost.Content);
    }
}
