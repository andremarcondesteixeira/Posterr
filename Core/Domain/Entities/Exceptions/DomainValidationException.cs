namespace Posterr.Core.Domain.Entities.Exceptions;

public abstract class DomainValidationException(string message) : DomainException(message)
{
}
