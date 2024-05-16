using Posterr.Core.Boundaries.Configuration;
using Posterr.Core.Domain.Entities.Exceptions;

namespace Posterr.Core.Domain.Entities.Publications.Exceptions;

public sealed class MaxPostContentLengthExceededException(int contentLength, IDomainConfig DomainConfig)
    : DomainValidationException(
        $"The post content is limited to {DomainConfig.MaxPostLength} characters. Got {contentLength} instead."
    )
{
}