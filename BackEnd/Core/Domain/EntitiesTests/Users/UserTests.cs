#pragma warning disable CS8604 // Possible null reference argument.

using Posterr.Core.Domain.Entities.Users;
using Posterr.Core.Shared.Exceptions;

namespace Posterr.Core.Domain.EntitiesTests.Users;

public class UserTests
{
    [Fact]
    public void GivenValidParameters_WhenInstantiatingUserEntity_ThenSucceed()
    {
        var user = new User(1, "username");
        Assert.Equal(1, user.Id);
        Assert.Equal("username", user.Username);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void GivenEmptyUsername_WhenInstantiatingUserEntity_ThenThrowException(string? username)
    {
        // This test will guarantee tha the User entity is not swallowing the exception
        Assert.Throws<InvalidUsernameException>(() => new User(1, username));
    }
}

