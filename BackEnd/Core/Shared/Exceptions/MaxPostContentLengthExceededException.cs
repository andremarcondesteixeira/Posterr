using Posterr.Core.Boundaries.ConfigurationInterface;

namespace Posterr.Core.Shared.Exceptions;

public sealed class MaxPostContentLengthExceededException(int contentLength, IDomainConfig domainConfig)
    : PosterrException(
        $"The post content is limited to {domainConfig.MaxPostLength} characters. Got {contentLength} instead.",
        $"Provide content having between 1 and {domainConfig.MaxPostLength} characters."
    )
{
}