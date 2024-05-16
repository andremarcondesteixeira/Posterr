using Posterr.Core.Domain.Entities.Exceptions;

namespace Posterr.Core.Domain.Entities.Users.Exceptions;

public sealed class InvalidUsernameException(string Value)
    : DomainValidationException(
        $"Username must be not empty and contain only alphanumeric characters. Got \"{Value}\", instead."
    )
{
}
