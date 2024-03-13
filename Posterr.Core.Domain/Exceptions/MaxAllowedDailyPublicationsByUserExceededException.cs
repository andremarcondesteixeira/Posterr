namespace Posterr.Core.Domain.Exceptions;

public class MaxAllowedDailyPublicationsByUserExceededException(string message) : DomainException(message)
{
}
