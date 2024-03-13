using Posterr.Core.Domain.Exceptions;

namespace Posterr.Core.Domain.Publications.Exceptions;

public sealed class MaxPostContentLengthExceededException(string message) : DomainValidationException(message)
{
}