using Posterr.Core.Domain.Exceptions;

namespace Posterr.Core.Application.Exceptions;

public sealed class UserNotFoundException(string username)
    : DomainValidationException($"No user was found with the username \"{username}\".")
{
}
