using Posterr.Core.Domain.Exceptions;

namespace Posterr.Core.Domain.Publications.Exceptions;

public sealed class MaxPostContentLengthExceededException(int contentLength, IDomainConfig DomainConfig)
    : DomainValidationException(
        $"The post content is limited to {DomainConfig.MaxPostLength} characters. Got {contentLength} instead."
    )
{
}