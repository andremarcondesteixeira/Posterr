using FakeItEasy;
using Posterr.Core.Domain.Publications;
using Posterr.Core.Domain.Users;

namespace Posterr.Core.Application.Tests;

static class Helpers
{
    public static IUser MakeDummyUser(string username)
    {
        var user = A.Fake<IUser>();
        A.CallTo(() => user.Username).Returns(username);
        return user;
    }

    public static IPost MakeDummyPost(long postId, IUser user, DateTime now, string content)
    {
        var post = A.Fake<IPost>();

        A.CallTo(() => post.Id).Returns(postId);
        A.CallTo(() => post.Author).Returns(user);
        A.CallTo(() => post.PublicationDate).Returns(now);
        A.CallTo(() => post.Content).Returns(content);

        return post;
    }

    public static IUnpublishedPost MakeDummyUnpublishedPost(IUser author, string content)
    {
        var unpublishedPost = A.Fake<IUnpublishedPost>();

        A.CallTo(() => unpublishedPost.Author).Returns(author);
        A.CallTo(() => unpublishedPost.Content).Returns(content);

        return unpublishedPost;
    }
}
