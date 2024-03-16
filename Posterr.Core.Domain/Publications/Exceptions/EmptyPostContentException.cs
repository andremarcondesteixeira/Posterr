using Posterr.Core.Domain.Exceptions;

namespace Posterr.Core.Domain.Publications.Exceptions;

public sealed class EmptyPostContentException() : DomainValidationException("The post content must not be empty.")
{
}
