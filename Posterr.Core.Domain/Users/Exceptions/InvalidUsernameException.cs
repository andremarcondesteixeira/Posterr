using Posterr.Core.Domain.Exceptions;

namespace Posterr.Core.Domain.Users.Exceptions;

public sealed class InvalidUsernameException(string Value)
    : DomainValidationException(
        $"Username must be not empty and contain only alphanumeric characters. Got \"{Value}\", instead."
    )
{
}
