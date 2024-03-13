namespace Posterr.Core.Domain.Exceptions;

public abstract class DomainValidationException(string message) : DomainException(message)
{
}
