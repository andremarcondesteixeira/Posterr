#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.

using FakeItEasy;
using Posterr.Core.Application.UseCases.CreateNewRepost;
using Posterr.Core.Shared.ConfigurationInterfaces;
using Posterr.Core.Shared.EntitiesInterfaces;
using Posterr.Core.Shared.Exceptions;
using Posterr.Core.Shared.PersistenceInterfaces;

namespace Posterr.Core.Application.UseCasesTests;

public class CreateNewRepostUseCaseTests
{
    private readonly IUsersRepository usersRepository = Fake.UserRepository();
    private readonly IPublicationsRepository publicationsRepository = Fake.PublicationsRepository();
    private readonly IDomainConfig domainConfig = Fake.DomainConfig();
    private readonly CreateNewRepostUseCase useCase;

    public CreateNewRepostUseCaseTests()
    {
        useCase = new CreateNewRepostUseCase(usersRepository, publicationsRepository, domainConfig);
    }

    [Fact]
    public void GivenCorrectParametersAndRequirements_WhenCreatingNewRepost_ThenSucceed()
    {
        var yesterday = Fake.CurrentTimeUTC.AddDays(-1);
        var originalPostAuthor = Fake.User(Fake.OriginalPostAuthorUsername);
        var originalPost = Fake.Post(1, originalPostAuthor, yesterday, Fake.Content);
        var repostAuthor = Fake.User(Fake.RepostAuthorUsername);
        var unpublishedRepost = Fake.UnpublishedRepost(repostAuthor, originalPost);
        var publishedRepost = Fake.Repost(2, unpublishedRepost.Author, Fake.CurrentTimeUTC, "repost content", unpublishedRepost.OriginalPost);
        A.CallTo(() => usersRepository.FindByUsername(repostAuthor.Username)).Returns(repostAuthor);
        A.CallTo(() => publicationsRepository.FindById(originalPost.Id)).Returns(originalPost);
        A.CallTo(() => publicationsRepository.CountRepostsByUserAndOriginalPost(A<IUser>.Ignored, A<IPost>.Ignored)).Returns(0);
        A.CallTo(() => publicationsRepository.CountPublicationsMadeByUserBetweenDateTimeRange(
            repostAuthor,
            A<DateTime>.Ignored,
            A<DateTime>.Ignored
        )).Returns(0);
        A.CallTo(() => publicationsRepository.PublishNewRepost(A<IUnpublishedRepost>.That.Matches(r =>
            r.Author.Username == unpublishedRepost.Author.Username
            && r.OriginalPost.Id == unpublishedRepost.OriginalPost.Id
            && r.OriginalPost.Author.Username == unpublishedRepost.OriginalPost.Author.Username
            && r.OriginalPost.PublicationDate == unpublishedRepost.OriginalPost.PublicationDate
            && r.OriginalPost.Content == unpublishedRepost.OriginalPost.Content
        ))).Returns(publishedRepost);

        var dto = new CreateNewRepostUseCaseInputDTO(repostAuthor.Username, "repost content", originalPost.Id);
        var response = useCase.Run(dto);

        Assert.Equal(2, response.Id);
        Assert.Equal(Fake.RepostAuthorUsername, response.Author.Username);
        Assert.Equal(Fake.CurrentTimeUTC, response.PublicationDate);
        Assert.Equal("repost content", response.Content);
        Assert.Equal(1, response.OriginalPost.Id);
        Assert.Equal(Fake.OriginalPostAuthorUsername, response.OriginalPost.Author.Username);
        Assert.Equal(yesterday, response.OriginalPost.PublicationDate);
        Assert.Equal(Fake.Content, response.OriginalPost.Content);
    }

    [Fact]
    public void GivenRepostAuthorUsernameDoesNotBelongToAnyRegisteredUser_WhenCreatingNewRepost_ThenThrowException()
    {
        A.CallTo(() => usersRepository.FindByUsername(Fake.Username)).Returns(null);
        var request = new CreateNewRepostUseCaseInputDTO(Fake.Username, "", 1);
        Assert.Throws<UserNotFoundException>(() => useCase.Run(request));
    }

    [Fact]
    public void GivenOriginalPostNotFound_WhenCreatingNewRepost_ThenThrowException()
    {
        var repostAuthor = Fake.User(Fake.Username);
        A.CallTo(() => usersRepository.FindByUsername(repostAuthor.Username)).Returns(repostAuthor);
        A.CallTo(() => publicationsRepository.FindById(1)).Returns(null);
        var request = new CreateNewRepostUseCaseInputDTO(repostAuthor.Username, "", 1);
        Assert.Throws<PostNotFoundException>(() => useCase.Run(request));
    }

    [Fact]
    public void GivenUserHasReachedMaxAllowedDailyPublications_WhenCreatingNewRepost_ThenThrowException()
    {
        var yesterday = Fake.CurrentTimeUTC.AddDays(-1);
        var originalPostAuthor = Fake.User(Fake.OriginalPostAuthorUsername);
        var originalPost = Fake.Post(1, originalPostAuthor, yesterday, Fake.Content);
        var repostAuthor = Fake.User(Fake.RepostAuthorUsername);
        A.CallTo(() => usersRepository.FindByUsername(repostAuthor.Username)).Returns(repostAuthor);
        A.CallTo(() => publicationsRepository.FindById(originalPost.Id)).Returns(originalPost);
        A.CallTo(() => publicationsRepository.CountRepostsByUserAndOriginalPost(A<IUser>.Ignored, A<IPost>.Ignored)).Returns(0);
        A.CallTo(() => publicationsRepository.CountPublicationsMadeByUserBetweenDateTimeRange(
            repostAuthor,
            A<DateTime>.Ignored,
            A<DateTime>.Ignored
        )).Returns(
            domainConfig.MaxAllowedDailyPublicationsByUser
        );

        var createNewRepostRequest = new CreateNewRepostUseCaseInputDTO(repostAuthor.Username, "", originalPost.Id);

        Assert.Throws<MaxAllowedDailyPublicationsByUserExceededException>(
            () => useCase.Run(createNewRepostRequest)
        );
    }

    [Fact]
    public void GivenAPublishedRepost_ThenTheRepostCannotBeReposted()
    {
        var yesterday = Fake.CurrentTimeUTC.AddDays(-1);
        var originalPostAuthor = Fake.User(Fake.OriginalPostAuthorUsername);
        var originalPost = Fake.Repost(2, originalPostAuthor, yesterday, "content", Fake.Post(1, originalPostAuthor, yesterday.AddHours(-1), "original"));
        var repostAuthor = Fake.User(Fake.RepostAuthorUsername);
        var createNewRepostRequest = new CreateNewRepostUseCaseInputDTO(repostAuthor.Username, "", originalPost.Id);
        A.CallTo(() => usersRepository.FindByUsername(repostAuthor.Username)).Returns(repostAuthor);
        A.CallTo(() => publicationsRepository.FindById(originalPost.Id)).Returns(originalPost);

        Assert.Throws<CannotRepostRepostException>(
            () => useCase.Run(createNewRepostRequest)
        );
    }
}
