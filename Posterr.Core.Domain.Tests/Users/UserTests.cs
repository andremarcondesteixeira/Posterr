using Posterr.Core.Domain.Users;
using Posterr.Core.Domain.Users.Exceptions;

namespace Posterr.Core.Domain.Tests.Users;

public class UserTests
{
    [Fact]
    public void GivenValidParameters_WhenInstantiatingUserEntity_ThenSucceed()
    {
        var user = new User("username");
        Assert.Equal("username", user.Username);
    }

#pragma warning disable CS8604 // Possible null reference argument.
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void GivenEmptyUsername_WhenInstantiatingUserEntity_ThenThrowException(string? username)
    {
        // This test will guarantee tha the User entity is not swallowing the exception
        Assert.Throws<InvalidUsernameException>(() => new User(username));
    }
#pragma warning restore CS8604 // Possible null reference argument.
}

