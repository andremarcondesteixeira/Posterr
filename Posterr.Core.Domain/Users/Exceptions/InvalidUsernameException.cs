using Posterr.Core.Domain.Exceptions;

namespace Posterr.Core.Domain.Users.Exceptions;

public sealed class InvalidUsernameException(string message) : DomainValidationException(message)
{
}
