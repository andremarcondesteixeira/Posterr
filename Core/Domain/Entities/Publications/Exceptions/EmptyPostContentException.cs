using Posterr.Core.Domain.Entities.Exceptions;

namespace Posterr.Core.Domain.Entities.Publications.Exceptions;

public sealed class EmptyPostContentException() : DomainValidationException("The post content must not be empty.")
{
}
