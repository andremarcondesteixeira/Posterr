using Posterr.Core.Domain.Entities.Exceptions;

namespace Posterr.Core.Application.UseCases.Exceptions;

public sealed class UserNotFoundException(string username)
    : DomainValidationException($"No user was found with the username \"{username}\".")
{
}
