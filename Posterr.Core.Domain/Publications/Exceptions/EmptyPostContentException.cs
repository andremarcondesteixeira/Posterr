using Posterr.Core.Domain.Exceptions;

namespace Posterr.Core.Domain.Publications.Exceptions;

public sealed class EmptyPostContentException(string message) : DomainValidationException(message)
{
}
