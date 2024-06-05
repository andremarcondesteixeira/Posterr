using FakeItEasy;
using Posterr.Core.Boundaries.ConfigurationInterface;
using Posterr.Core.Boundaries.EntitiesInterfaces;
using Posterr.Core.Boundaries.Persistence;

namespace Posterr.Core.Application.UseCasesTests;

static class Fake
{
    public const string Content = "content";
    public static readonly DateTime CurrentTimeUTC = DateTime.UtcNow;
    public const string OriginalPostAuthorUsername = "original_post_author_username";
    public const string RepostAuthorUsername = "repost_author_username";
    public const string Username = "username";

    public static IDomainConfig DomainConfig()
    {
        var domainConfig = A.Fake<IDomainConfig>(x => x.Strict(StrictFakeOptions.AllowToString));
        var pagination = A.Fake<IDomainConfig.IPaginationConfig>(x => x.Strict(StrictFakeOptions.AllowToString));

        A.CallTo(() => domainConfig.MaxPostLength).Returns((uint)7);
        A.CallTo(() => domainConfig.MaxAllowedDailyPublicationsByUser).Returns((ushort)5);
        A.CallTo(() => domainConfig.Pagination).Returns(pagination);
        A.CallTo(() => pagination.FirstPageSize).Returns((short)15);
        A.CallTo(() => pagination.NextPagesSize).Returns((short)20);

        return domainConfig;
    }

    public static IPost Post(long postId, IUser user, DateTime now, string content)
    {
        var post = A.Fake<IPost>();

        A.CallTo(() => post.Id).Returns(postId);
        A.CallTo(() => post.Author).Returns(user);
        A.CallTo(() => post.PublicationDate).Returns(now);
        A.CallTo(() => post.Content).Returns(content);

        return post;
    }

    public static IPublicationsRepository PublicationsRepository() => A.Fake<IPublicationsRepository>(x => x.Strict());

    public static IRepost Repost(long id, IUser author, DateTime publicationDate, string content, IPost originalPost)
    {
        var repost = A.Fake<IRepost>();

        A.CallTo(() => repost.Id).Returns(id);
        A.CallTo(() => repost.Author).Returns(author);
        A.CallTo(() => repost.PublicationDate).Returns(publicationDate);
        A.CallTo(() => repost.Content).Returns(content);
        A.CallTo(() => repost.OriginalPost).Returns(originalPost);

        return repost;
    }

    public static IUnpublishedPost UnpublishedPost(IUser author, string content)
    {
        var unpublishedPost = A.Fake<IUnpublishedPost>();

        A.CallTo(() => unpublishedPost.Author).Returns(author);
        A.CallTo(() => unpublishedPost.Content).Returns(content);

        return unpublishedPost;
    }

    public static IUnpublishedRepost UnpublishedRepost(IUser author, IPost originalPost)
    {
        var unpublishedRepost = A.Fake<IUnpublishedRepost>();

        A.CallTo(() => unpublishedRepost.Author).Returns(author);
        A.CallTo(() => unpublishedRepost.OriginalPost).Returns(originalPost);

        return unpublishedRepost;
    }

    public static IUser User(string username)
    {
        var user = A.Fake<IUser>();
        A.CallTo(() => user.Username).Returns(username);
        return user;
    }

    public static IUsersRepository UserRepository() => A.Fake<IUsersRepository>(x => x.Strict());
}
