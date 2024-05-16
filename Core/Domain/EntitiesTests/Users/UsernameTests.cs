#pragma warning disable CS8604 // Possible null reference argument.

using Posterr.Core.Domain.Entities.Users;
using Posterr.Core.Domain.Entities.Users.Exceptions;

namespace Posterr.Core.Domain.EntitiesTests.Users;

public class UsernameTests
{
    [Fact]
    public void GivenUsernameContainsOnlyAlphanumericCharacters_WhenCreatingUsernameValueObject_ThenSucceed()
    {
        var username = new Username("username");
        Assert.Equal("username", username.Value);
        Assert.Equal("username", username.ToString());
    }

    [Fact]
    public void GivenUsernameContainsNonAlphanumericCharacters_WhenCreatingUsernameValueObject_ThenFail()
    {
        Assert.Throws<InvalidUsernameException>(() => new Username("u$ern@me"));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void GivenUsernameIsNullOrEmpty_WhenCreatingUsernameValueObject_ThenFail(string? username)
    {
        Assert.Throws<InvalidUsernameException>(() => new Username(username));
    }
}
